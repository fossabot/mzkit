﻿Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula.ElementsExactMass
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula.MS
Imports BioNovoGene.BioDeep.MSEngine

Public Class LipidSpectrumGeneratorFactory
    Public Function Create(lipidClass As LbmClass, adduct As AdductIon, rules As ISpectrumGenerationRule()) As ILipidSpectrumGenerator
        Return New RuleBaseSpectrumGenerator(lipidClass, adduct, rules)
    End Function
End Class

Friend Class RuleBaseSpectrumGenerator
    Implements ILipidSpectrumGenerator
    Public Sub New(lipidClass As LbmClass, adduct As AdductIon, rules As ISpectrumGenerationRule())
        Me.LipidClass = lipidClass
        Me.Adduct = adduct
        Me.Rules = rules
    End Sub

    Public ReadOnly Property LipidClass As LbmClass
    Public ReadOnly Property Adduct As AdductIon
    Public ReadOnly Property Rules As ISpectrumGenerationRule()

    Public Function CanGenerate(lipid As ILipid, adduct As AdductIon) As Boolean Implements ILipidSpectrumGenerator.CanGenerate
        Return lipid.LipidClass = LipidClass AndAlso Equals(adduct.AdductIonName, adduct.AdductIonName)
    End Function

    Public Function Generate(lipid As Lipid, adduct As AdductIon, Optional molecule As IMoleculeProperty = Nothing) As IMSScanProperty Implements ILipidSpectrumGenerator.Generate
        If Not CanGenerate(lipid, adduct) Then
            Return Nothing
        End If
        Dim spectrum = Rules.SelectMany(Function(rule) rule.Create(lipid, adduct)).GroupBy(Function(spec) spec, comparer).[Select](Function(specs) New SpectrumPeak(Enumerable.First(specs).mz, Enumerable.First(specs).Intensity, String.Join(", ", specs.[Select](Function(spec) spec.Annotation)))).OrderBy(Function(peak) peak.mz).ToList()

        Return New MoleculeMsReference With {
.PrecursorMz = adduct.ConvertToMz(lipid.Mass),
.IonMode = adduct.IonMode,
.Spectrum = spectrum,
.Name = lipid.Name,
.Formula = molecule?.Formula,
.Ontology = molecule?.Ontology,
.SMILES = molecule?.SMILES,
.InChIKey = molecule?.InChIKey,
.AdductType = adduct,
.CompoundClass = lipid.LipidClass.ToString(),
.Charge = adduct.ChargeNumber
}
    End Function

    Private Shared ReadOnly comparer As IEqualityComparer(Of SpectrumPeak) = New SpectrumEqualityComparer()
End Class

Public Interface ISpectrumGenerationRule
    Function Create(lipid As ILipid, adduct As AdductIon) As SpectrumPeak()
End Interface

Public Class MzVariableRule
    Implements ISpectrumGenerationRule
    Public Sub New(mz As IMzVariable, intensity As Double, comment As String)
        Me.Mz = mz
        Me.Intensity = intensity
        Me.Comment = comment
    End Sub

    Public ReadOnly Property Mz As IMzVariable
    Public ReadOnly Property Intensity As Double
    Public ReadOnly Property Comment As String

    Public Function Create(lipid As ILipid, adduct As AdductIon) As SpectrumPeak() Implements ISpectrumGenerationRule.Create
        Return Mz.Evaluate(lipid, adduct).[Select](Function(mz) New SpectrumPeak(mz, Intensity, Comment)).ToArray()
    End Function
End Class

Public Interface IMzVariable
    Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double)
End Interface

Public Class EmptyMz
    Implements IMzVariable
    Public Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        Return Enumerable.Empty(Of Double)()
    End Function
End Class

Public Class ConstantMz
    Implements IMzVariable
    Public Sub New(exactMass As Double)
        Me.ExactMass = exactMass
    End Sub

    Public ReadOnly Property ExactMass As Double

    Public Iterator Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        Yield ExactMass
    End Function

    Public Overrides Function ToString() As String
        Return $"Const: {ExactMass}"
    End Function
End Class

Public Class PrecursorMz
    Implements IMzVariable
    Public Iterator Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        Yield adduct.ConvertToMz(lipid.Mass)
    End Function

    Public Overrides Function ToString() As String
        Return $"Precursor m/z"
    End Function
End Class

Public Class MolecularLevelChains
    Implements IMzVariable
    Public Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        Dim lipid_ As Lipid = TryCast(lipid, Lipid)
        Dim chains As SeparatedChains = TryCast(lipid_.Chains, SeparatedChains)

        If lipid_ IsNot Nothing AndAlso chains IsNot Nothing Then
            Return lipid.Chains.GetDeterminedChains().[Select](Function(chain) chain.Mass)
        End If
        Return Enumerable.Empty(Of Double)()
    End Function
End Class

Public Class PositionChainMz
    Implements IMzVariable
    Public Sub New(position As Integer)
        Me.Position = position
    End Sub

    Public ReadOnly Property Position As Integer

    Public Iterator Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        Dim lipid_ As Lipid = TryCast(lipid, Lipid)
        Dim chains As PositionLevelChains = TryCast(lipid_.Chains, PositionLevelChains)

        If lipid_ IsNot Nothing AndAlso chains IsNot Nothing AndAlso chains.ChainCount >= Position Then
            Dim chain = lipid.Chains.GetChainByPosition(Position)
            Yield chain.Mass
        End If
    End Function
End Class

Public Class ChainDesorptionMz
    Implements IMzVariable
    Public Sub New(position As Integer)
        Me.Position = position
    End Sub

    Public ReadOnly Property Position As Integer

    Public Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        Dim lipid_ As Lipid = TryCast(lipid, Lipid)
        Dim chains As SeparatedChains = TryCast(lipid_.Chains, SeparatedChains)

        If lipid_ IsNot Nothing AndAlso chains IsNot Nothing AndAlso chains.ChainCount >= Position Then
            Return CreateSpectrum(lipid.Chains.GetChainByPosition(Position))
        End If
        Return Enumerable.Empty(Of Double)()
    End Function

    Private Function CreateSpectrum(chain As IChain) As IEnumerable(Of Double)
        If chain.CarbonCount = 0 OrElse chain.DoubleBond.UnDecidedCount <> 0 OrElse chain.Oxidized.UnDecidedCount <> 0 Then
            Return New Double(-1) {}
        End If
        Dim diffs = New Double(chain.CarbonCount - 1) {}
        For i = 0 To chain.CarbonCount - 1
            diffs(i) = HydrogenMass * 2 + CarbonMass
        Next
        For Each bond In chain.DoubleBond.Bonds
            diffs(bond.Position - 1) -= HydrogenMass
            diffs(bond.Position) -= HydrogenMass
        Next
        For Each ox In chain.Oxidized.Oxidises
            diffs(ox - 1) += OxygenMass
        Next
        Dim acylChain As AcylChain = TryCast(chain, AcylChain)

        If acylChain IsNot Nothing Then
            diffs(0) += OxygenMass - HydrogenMass * 2
        End If
        For i = 1 To chain.CarbonCount - 1
            diffs(i) += diffs(i - 1)
        Next
        Return diffs.Take(chain.CarbonCount - 1).[Select](Function(diff) chain.Mass - diff)
    End Function
End Class

Public Class LossMz
    Implements IMzVariable
    Public Sub New(left As IMzVariable, right As IMzVariable)
        Me.Left = left
        Me.Right = right
    End Sub

    Public ReadOnly Property Left As IMzVariable
    Public ReadOnly Property Right As IMzVariable

    Public Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        Return Left.Evaluate(lipid, adduct).SelectMany(Function(__) Right.Evaluate(lipid, adduct), Function(left, right) left - right)
    End Function
End Class

Public Class MzVariableProxy
    Implements IMzVariable
    Public Sub New(mz As IMzVariable)
        Me.Mz = mz
    End Sub

    Public ReadOnly Property Mz As IMzVariable

    Private cache As Double() = New Double(-1) {}

    Private lipid As ILipid
    Private adduct As AdductIon

    Public Function Evaluate(lipid As ILipid, adduct As AdductIon) As IEnumerable(Of Double) Implements IMzVariable.Evaluate
        If lipid Is Me.lipid AndAlso adduct Is Me.adduct Then
            Return cache
        End If
        Me.lipid = lipid
        Me.adduct = adduct
        Me.cache = Mz.Evaluate(lipid, adduct).ToArray()

        Return cache
    End Function
End Class

