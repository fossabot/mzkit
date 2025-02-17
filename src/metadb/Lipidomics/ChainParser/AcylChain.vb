﻿Imports BioNovoGene.BioDeep.Chemoinformatics.Formula.ElementsExactMass

Public Class AcylChain : Implements IChain

    Public Sub New(carbonCount As Integer, doubleBond As IDoubleBond, oxidized As IOxidized)
        Me.CarbonCount = carbonCount
        Me.DoubleBond = doubleBond
        Me.Oxidized = oxidized
    End Sub

    Public ReadOnly Property DoubleBond As IDoubleBond Implements IChain.DoubleBond

    Public ReadOnly Property Oxidized As IOxidized Implements IChain.Oxidized

    Public ReadOnly Property CarbonCount As Integer Implements IChain.CarbonCount

    Public ReadOnly Property DoubleBondCount As Integer Implements IChain.DoubleBondCount
        Get
            Return DoubleBond.Count
        End Get
    End Property

    Public ReadOnly Property OxidizedCount As Integer Implements IChain.OxidizedCount
        Get
            Return Oxidized.Count
        End Get
    End Property

    Public ReadOnly Property Mass As Double Implements IChain.Mass
        Get
            Return CalculateAcylMass(CarbonCount, DoubleBondCount, OxidizedCount)
        End Get
    End Property

    Public Function GetCandidates(generator As IChainGenerator) As IEnumerable(Of IChain) Implements IChain.GetCandidates
        Return generator.Generate(Me)
    End Function

    Public Overrides Function ToString() As String
        Return $"{CarbonCount}:{FormatDoubleBond(DoubleBond)}{Oxidized}"
    End Function

    Private Shared Function FormatDoubleBond(doubleBond As IDoubleBond) As String
        If doubleBond.DecidedCount >= 1 Then
            Return $"{doubleBond.Count}({String.Join(",", doubleBond.Bonds)})"
        Else
            Return doubleBond.Count.ToString()
        End If
    End Function

    Private Shared Function CalculateAcylMass(carbon As Integer, doubleBond As Integer, oxidize As Integer) As Double
        If carbon = 0 AndAlso doubleBond = 0 AndAlso oxidize = 0 Then
            Return HydrogenMass
        End If
        Return carbon * CarbonMass + (2 * carbon - 2 * doubleBond - 1) * HydrogenMass + (1 + oxidize) * OxygenMass
    End Function

    Public Function Accept(Of TResult)(visitor As IAcyclicVisitor, decomposer As IAcyclicDecomposer(Of TResult)) As TResult Implements IVisitableElement.Accept
        Dim concrete As IDecomposer(Of TResult, AcylChain) = TryCast(decomposer, IDecomposer(Of TResult, AcylChain))

        If concrete IsNot Nothing Then
            Return concrete.Decompose(visitor, Me)
        End If
        Return Nothing
    End Function

    Public Function Includes(chain As IChain) As Boolean Implements IChain.Includes
        Return TypeOf chain Is AcylChain AndAlso chain.CarbonCount = CarbonCount AndAlso chain.DoubleBondCount = DoubleBondCount AndAlso chain.OxidizedCount = OxidizedCount AndAlso DoubleBond.Includes(chain.DoubleBond) AndAlso Oxidized.Includes(chain.Oxidized)
    End Function

    Public Overloads Function Equals(other As IChain) As Boolean Implements IEquatable(Of IChain).Equals
        Return TypeOf other Is AcylChain AndAlso CarbonCount = other.CarbonCount AndAlso DoubleBond.Equals(other.DoubleBond) AndAlso Oxidized.Equals(other.Oxidized)
    End Function
End Class

Public Class AlkylChain
    Implements IChain
    Public Sub New(carbonCount As Integer, doubleBond As IDoubleBond, oxidized As IOxidized)
        Me.CarbonCount = carbonCount
        Me.DoubleBond = doubleBond
        Me.Oxidized = oxidized
    End Sub
    Public ReadOnly Property DoubleBond As IDoubleBond Implements IChain.DoubleBond

    Public ReadOnly Property Oxidized As IOxidized Implements IChain.Oxidized

    Public ReadOnly Property CarbonCount As Integer Implements IChain.CarbonCount

    Public ReadOnly Property DoubleBondCount As Integer Implements IChain.DoubleBondCount
        Get
            Return DoubleBond.Count
        End Get
    End Property

    Public ReadOnly Property OxidizedCount As Integer Implements IChain.OxidizedCount
        Get
            Return Oxidized.Count
        End Get
    End Property

    Public ReadOnly Property Mass As Double Implements IChain.Mass
        Get
            Return CalculateAlkylMass(CarbonCount, DoubleBondCount, OxidizedCount)
        End Get
    End Property

    Private Shared Function CalculateAlkylMass(carbon As Integer, doubleBond As Integer, oxidize As Integer) As Double
        Return carbon * CarbonMass + (2 * carbon - 2 * doubleBond + 1) * HydrogenMass + oxidize * OxygenMass
    End Function

    Public Function GetCandidates(generator As IChainGenerator) As IEnumerable(Of IChain) Implements IChain.GetCandidates
        Return generator.Generate(Me)
    End Function

    Public Overrides Function ToString() As String
        If IsPlasmalogen Then
            Return $"P-{CarbonCount}:{FormatDoubleBondWhenPlasmalogen(DoubleBond)}{Oxidized}"
        Else
            Return $"O-{CarbonCount}:{FormatDoubleBond(DoubleBond)}{Oxidized}"
        End If
    End Function

    Public ReadOnly Property IsPlasmalogen As Boolean
        Get
            Return DoubleBond.Bonds.Any(Function(b) b.Position = 1)
        End Get
    End Property

    Private Shared Function FormatDoubleBond(doubleBond As IDoubleBond) As String
        If doubleBond.DecidedCount >= 1 Then
            Return $"{doubleBond.Count}({String.Join(",", doubleBond.Bonds)})"
        Else
            Return doubleBond.Count.ToString()
        End If
    End Function

    Private Shared Function FormatDoubleBondWhenPlasmalogen(doubleBond As IDoubleBond) As String
        If doubleBond.DecidedCount > 1 Then
            Return $"{doubleBond.Count - 1}({String.Join(",", doubleBond.Bonds.Where(Function(b) b.Position <> 1))})"
        ElseIf doubleBond.DecidedCount = 1 Then
            Return $"{doubleBond.Count - 1}"
        Else
            Throw New ArgumentException("Plasmalogens must have more than 1 double bonds.")
        End If
    End Function

    Public Function Accept(Of TResult)(visitor As IAcyclicVisitor, decomposer As IAcyclicDecomposer(Of TResult)) As TResult Implements IVisitableElement.Accept
        Dim concrete As IDecomposer(Of TResult, AlkylChain) = TryCast(decomposer, IDecomposer(Of TResult, AlkylChain))

        If concrete IsNot Nothing Then
            Return concrete.Decompose(visitor, Me)
        End If
        Return Nothing
    End Function

    Public Function Includes(chain As IChain) As Boolean Implements IChain.Includes
        Return TypeOf chain Is AlkylChain AndAlso chain.CarbonCount = CarbonCount AndAlso chain.DoubleBondCount = DoubleBondCount AndAlso chain.OxidizedCount = OxidizedCount AndAlso DoubleBond.Includes(chain.DoubleBond) AndAlso Oxidized.Includes(chain.Oxidized)
    End Function

    Public Overloads Function Equals(other As IChain) As Boolean Implements IEquatable(Of IChain).Equals
        Return TypeOf other Is AlkylChain AndAlso CarbonCount = other.CarbonCount AndAlso DoubleBond.Equals(other.DoubleBond) AndAlso Oxidized.Equals(other.Oxidized)
    End Function
End Class

Public Class SphingoChain
    Implements IChain
    Public Sub New(carbonCount As Integer, doubleBond As IDoubleBond, oxidized As IOxidized)
        If oxidized Is Nothing Then
            Throw New ArgumentNullException(NameOf(oxidized))
        End If
        'if (!oxidized.Oxidises.Contains(1) || !oxidized.Oxidises.Contains(3))
        '{
        'if (!oxidized.Oxidises.Contains(1))
        '{
        '    throw new ArgumentException(nameof(oxidized));
        '}

        Me.CarbonCount = carbonCount
        Me.DoubleBond = doubleBond
        Me.Oxidized = oxidized
    End Sub

    Public ReadOnly Property CarbonCount As Integer Implements IChain.CarbonCount

    Public ReadOnly Property DoubleBond As IDoubleBond Implements IChain.DoubleBond

    Public ReadOnly Property Oxidized As IOxidized Implements IChain.Oxidized

    Public ReadOnly Property DoubleBondCount As Integer Implements IChain.DoubleBondCount
        Get
            Return DoubleBond.Count
        End Get
    End Property

    Public ReadOnly Property OxidizedCount As Integer Implements IChain.OxidizedCount
        Get
            Return Oxidized.Count
        End Get
    End Property

    Public ReadOnly Property Mass As Double Implements IChain.Mass
        Get
            Return CalculateSphingosineMass(CarbonCount, DoubleBondCount, OxidizedCount)
        End Get
    End Property

    Private Shared Function CalculateSphingosineMass(carbon As Integer, doubleBond As Integer, oxidize As Integer) As Double
        Return carbon * CarbonMass + (2 * carbon - 2 * doubleBond + 2) * HydrogenMass + oxidize * OxygenMass + NitrogenMass
    End Function

    Public Function GetCandidates(generator As IChainGenerator) As IEnumerable(Of IChain) Implements IChain.GetCandidates
        Return generator.Generate(Me)
    End Function

    Public Function Accept(Of TResult)(visitor As IAcyclicVisitor, decomposer As IAcyclicDecomposer(Of TResult)) As TResult Implements IVisitableElement.Accept
        Dim concrete As IDecomposer(Of TResult, SphingoChain) = TryCast(decomposer, IDecomposer(Of TResult, SphingoChain))

        If concrete IsNot Nothing Then
            Return concrete.Decompose(visitor, Me)
        End If
        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return $"{CarbonCount}:{DoubleBond}{Oxidized}"
        'var OxidizedcountString = OxidizedCount == 1 ? ";O" : ";O" + OxidizedCount.ToString();
        'return $"{CarbonCount}:{DoubleBond}{OxidizedcountString}";
    End Function

    Public Function Includes(chain As IChain) As Boolean Implements IChain.Includes
        Return TypeOf chain Is SphingoChain AndAlso chain.CarbonCount = CarbonCount AndAlso chain.DoubleBondCount = DoubleBondCount AndAlso chain.OxidizedCount = OxidizedCount AndAlso DoubleBond.Includes(chain.DoubleBond) AndAlso Oxidized.Includes(chain.Oxidized)
    End Function

    Public Overloads Function Equals(other As IChain) As Boolean Implements IEquatable(Of IChain).Equals
        Return TypeOf other Is SphingoChain AndAlso CarbonCount = other.CarbonCount AndAlso DoubleBond.Equals(other.DoubleBond) AndAlso Oxidized.Equals(other.Oxidized)
    End Function
End Class
