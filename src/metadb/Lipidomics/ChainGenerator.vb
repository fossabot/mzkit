﻿Imports std = System.Math

Public Class ChainGenerator
    Implements IChainGenerator
    Public Sub New(Optional begin As Integer = 3, Optional [end] As Integer = 3, Optional skip As Integer = 3)
        Me.Begin = begin
        Me.End = [end]
        Me.Skip = skip
    End Sub

    Public ReadOnly Property Begin As Integer ' if begin is 3, first double bond is 3-4 at the earliest counting from ketone carbon.
    Public ReadOnly Property [End] As Integer ' if end is 3 and number of carbon is 18, last double bond is 15-16 at latest.
    Public ReadOnly Property Skip As Integer ' if skip is 3 and 6-7 is double bond, next one is 9-10 at the earliest.

    Public Function Generate(chain As AcylChain) As IEnumerable(Of IChain) Implements IChainGenerator.Generate
        Dim bs = EnumerateBonds(chain.CarbonCount, chain.DoubleBond).ToArray()
        Dim os = EnumerateOxidized(chain.CarbonCount, chain.Oxidized, 2).ToArray()
        Return bs.SelectMany(Function(__) os, Function(b, o) New AcylChain(chain.CarbonCount, b, o))
    End Function

    Public Function Generate(chain As AlkylChain) As IEnumerable(Of IChain) Implements IChainGenerator.Generate
        Dim bs = EnumerateBonds(chain.CarbonCount, chain.DoubleBond).ToArray()
        Dim os = EnumerateOxidized(chain.CarbonCount, chain.Oxidized, 2).ToArray()
        Return bs.SelectMany(Function(__) os, Function(b, o) New AlkylChain(chain.CarbonCount, b, o))
    End Function

    Public Function Generate(chain As SphingoChain) As IEnumerable(Of IChain) Implements IChainGenerator.Generate
        Dim bs = EnumerateBonds(chain.CarbonCount, chain.DoubleBond).ToArray()
        Dim os = EnumerateOxidized(chain.CarbonCount, chain.Oxidized, 4).ToArray()
        Return bs.SelectMany(Function(__) os, Function(b, o) New SphingoChain(chain.CarbonCount, b, o))
    End Function

    Private Function EnumerateBonds(carbon As Integer, doubleBond As IDoubleBond) As IEnumerable(Of IDoubleBond)
        If doubleBond.UnDecidedCount = 0 Then
            Return {doubleBond}
        End If
        Dim used = New Boolean(carbon - 1) {}
        For i = 1 To Begin - 1
            used(i - 1) = True
        Next
        For Each bond In doubleBond.Bonds
            For i = std.Max(1, bond.Position - Skip + 1) To bond.Position + Skip - 1
                used(i - 1) = True
            Next
        Next

        Dim rec As Func(Of Integer, List(Of IDoubleBondInfo), IEnumerable(Of IDoubleBond)) =
            Iterator Function(i As Integer, infos As List(Of IDoubleBondInfo)) As IEnumerable(Of IDoubleBond)
                If (infos.Count = doubleBond.UnDecidedCount) Then
                    Yield New DoubleBond(doubleBond.Bonds.Concat(infos).OrderBy(Function(b) b.Position).ToArray())
                    Return
                End If

                For j = i To carbon - Me.End
                    If (used(j - 1)) Then Continue For

                    infos.Add(DoubleBondInfo.Create(j))
                    For Each res In rec(j + Me.Skip, infos)
                        Yield res
                    Next
                    infos.RemoveAt(infos.Count - 1)
                Next
            End Function

        Return rec(Begin, New List(Of IDoubleBondInfo)(doubleBond.UnDecidedCount))
    End Function

    Private Function EnumerateOxidized(carbon As Integer, oxidized As IOxidized, begin As Integer) As IEnumerable(Of IOxidized)
        If oxidized.UnDecidedCount = 0 Then
            Return {oxidized}
        End If

        Dim rec As Func(Of Integer, List(Of Integer), IEnumerable(Of IOxidized)) =
            Iterator Function(i As Integer, infos As List(Of Integer)) As IEnumerable(Of IOxidized)
                If (infos.Count = oxidized.UnDecidedCount) Then
                    Yield Lipidomics.Oxidized.CreateFromPosition(oxidized.Oxidises.Concat(infos).OrderBy(Function(p) p).ToArray())
                    Return
                End If
                For j = i To carbon
                    If (oxidized.Oxidises.Contains(j)) Then Continue For

                    infos.Add(j)
                    For Each res In rec(j + 1, infos)
                        Yield res
                    Next
                    infos.RemoveAt(infos.Count - 1)
                Next
            End Function

        Return rec(begin, New List(Of Integer)(oxidized.UnDecidedCount))
    End Function

    Public Function CarbonIsValid(carbon As Integer) As Boolean Implements IChainGenerator.CarbonIsValid
        Return True
    End Function

    Public Function DoubleBondIsValid(carbon As Integer, db As Integer) As Boolean Implements IChainGenerator.DoubleBondIsValid
        Return db = 0 OrElse carbon >= Begin + Skip * (db - 1) + [End]
    End Function
End Class
