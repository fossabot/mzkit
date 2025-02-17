﻿Public Class PositionLevelChains : Inherits SeparatedChains
    Implements ITotalChain

    Private ReadOnly _chains As IChain()

    Public Sub New(ParamArray chains As IChain())
        MyBase.New(chains.[Select](Function(c, i) (c, i)).ToArray(), LipidDescription.Class Or LipidDescription.Chain Or LipidDescription.SnPosition)
        _chains = chains
    End Sub

    Private Function GetChainByPosition(position As Integer) As IChain Implements ITotalChain.GetChainByPosition
        If position > _chains.Length Then
            Return Nothing
        End If
        Return _chains(position - 1)
    End Function

    Private Function GetCandidateSets(totalChainGenerator As ITotalChainVariationGenerator) As IEnumerable(Of ITotalChain) Implements ITotalChain.GetCandidateSets
        Return totalChainGenerator.Product(Me)
    End Function

    Public Overrides Function ToString() As String
        Return String.Join("/", GetDeterminedChains().[Select](Function(c) c.ToString()))
    End Function

    Private Function Includes(chains As ITotalChain) As Boolean Implements ITotalChain.Includes
        Dim pChains As PositionLevelChains = TryCast(chains, PositionLevelChains)
        Return chains.ChainCount = ChainCount AndAlso
            pChains IsNot Nothing AndAlso
            Enumerable.Range(0, ChainCount).All(Function(i) GetDeterminedChains()(i).Includes(pChains.GetDeterminedChains()(i)))
    End Function

    Public Overrides Function Equals(other As ITotalChain) As Boolean Implements IEquatable(Of ITotalChain).Equals
        Dim pChains As PositionLevelChains = TryCast(other, PositionLevelChains)
        Return pChains IsNot Nothing AndAlso
            ChainCount = other.ChainCount AndAlso
            CarbonCount = other.CarbonCount AndAlso
            DoubleBondCount = other.DoubleBondCount AndAlso
            OxidizedCount = other.OxidizedCount AndAlso
            Description = other.Description AndAlso
            GetDeterminedChains().Zip(pChains.GetDeterminedChains(), Function(a, b) a.Equals(b)).All(Function(p) p)
    End Function

    Public Overrides Function Accept(Of TResult)(visitor As IAcyclicVisitor, decomposer As IAcyclicDecomposer(Of TResult)) As TResult Implements IVisitableElement.Accept
        Dim decomposer_ As IDecomposer(Of TResult, PositionLevelChains) = TryCast(decomposer, IDecomposer(Of TResult, PositionLevelChains))

        If decomposer_ IsNot Nothing Then
            Return decomposer_.Decompose(visitor, Me)
        End If
        Return Nothing
    End Function

End Class

