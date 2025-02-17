﻿Public Interface IChain
    Inherits IVisitableElement, IEquatable(Of IChain)
    ReadOnly Property CarbonCount As Integer
    ReadOnly Property DoubleBond As IDoubleBond
    ReadOnly Property Oxidized As IOxidized
    ReadOnly Property DoubleBondCount As Integer
    ReadOnly Property OxidizedCount As Integer
    ReadOnly Property Mass As Double

    Function Includes(chain As IChain) As Boolean
    Function GetCandidates(generator As IChainGenerator) As IEnumerable(Of IChain)
End Interface

