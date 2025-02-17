﻿
Friend NotInheritable Class ChainVisitorBuilder
    Implements ILipidomicsVisitorBuilder
    Private _acylDoubleBondState, _alkylDoubleBondState, _sphingoDoubleBondState As DoubleBondIndeterminateState
    Private _acylOxidizedState, _alkylOxidizedState, _sphingoOxidizedState As OxidizedIndeterminateState

    Public Function Create() As ChainVisitor
        Dim acylVisitor = New AcylChainVisitor(_acylDoubleBondState.AsVisitor(), _acylOxidizedState.AsVisitor())
        Dim alkylVisitor = New AlkylChainVisitor(_alkylDoubleBondState.AsVisitor(), _alkylOxidizedState.AsVisitor())
        Dim sphingoVisitor = New SphingosineChainVisitor(_sphingoDoubleBondState.AsVisitor(), _sphingoOxidizedState.AsVisitor())
        Return New ChainVisitor(acylVisitor, alkylVisitor, sphingoVisitor)
    End Function

    Private Sub SetChainsState(state As ChainsIndeterminateState) Implements ILipidomicsVisitorBuilder.SetChainsState
    End Sub

    Private Sub SetAcylDoubleBond(state As DoubleBondIndeterminateState) Implements ILipidomicsVisitorBuilder.SetAcylDoubleBond
        _acylDoubleBondState = state
    End Sub
    Private Sub SetAcylOxidized(state As OxidizedIndeterminateState) Implements ILipidomicsVisitorBuilder.SetAcylOxidized
        _acylOxidizedState = state
    End Sub

    Private Sub SetAlkylDoubleBond(state As DoubleBondIndeterminateState) Implements ILipidomicsVisitorBuilder.SetAlkylDoubleBond
        _alkylDoubleBondState = state
    End Sub
    Private Sub SetAlkylOxidized(state As OxidizedIndeterminateState) Implements ILipidomicsVisitorBuilder.SetAlkylOxidized
        _alkylOxidizedState = state
    End Sub

    Private Sub SetSphingoDoubleBond(state As DoubleBondIndeterminateState) Implements ILipidomicsVisitorBuilder.SetSphingoDoubleBond
        _sphingoDoubleBondState = state
    End Sub
    Private Sub SetSphingoOxidized(state As OxidizedIndeterminateState) Implements ILipidomicsVisitorBuilder.SetSphingoOxidized
        _sphingoOxidizedState = state
    End Sub

End Class
