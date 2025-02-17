﻿Imports System.Runtime.InteropServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports Microsoft.VisualBasic.DataMining.BinaryTree
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' a helper module for create the spectrum tree alignment
''' </summary>
Public Class NetworkingTree

    Friend ReadOnly align As MSScoreGenerator
    Friend ReadOnly equals_cutoff As Double = 0.85
    Friend ReadOnly diff As Double = 0.1

    Sub New(Optional mzdiff As Double = 0.3,
            Optional intocutoff As Double = 0.05,
            Optional equals As Double = 0.85,
            Optional interval As Double = 0.1)

        align = CreateAlignment(mzdiff, intocutoff, equals)
        equals_cutoff = equals
        diff = interval
    End Sub

    Public Shared Function CreateAlignment(Optional mzdiff As Double = 0.3,
                                           Optional intocutoff As Double = 0.05,
                                           Optional equals As Double = 0.85) As MSScoreGenerator
        Dim cosine As New CosAlignment(
            mzwidth:=Tolerance.DeltaMass(mzdiff),
            intocutoff:=New RelativeIntensityCutoff(intocutoff)
        )
        ' the align score generator didn't has
        ' any spectrum inside
        Return New MSScoreGenerator(cosine, equals, equals)
    End Function

    ''' <summary>
    ''' clear the spectrum cache
    ''' </summary>
    ''' <returns></returns>
    Public Function Clear() As NetworkingTree
        Call align.Clear()
        Return Me
    End Function

    Public Sub Add(spec As PeakMs2)
        Call align.Add(spec)
    End Sub

    ''' <summary>
    ''' do spectrum tree alignment
    ''' </summary>
    ''' <param name="ions"></param>
    ''' <returns></returns>
    Public Function Tree(ions As IEnumerable(Of PeakMs2)) As TreeCluster
        Dim ionsList As New List(Of PeakMs2)
        Dim clustering As New ClusterTree
        Dim clusters As New List(Of String)
        Dim class_id As String
        Dim args As New ClusterTree.Argument With {
            .alignment = align,
            .threshold = equals_cutoff,
            .diff = diff
        }

        For Each ion As PeakMs2 In ions.SafeQuery
            Call ionsList.Add(ion)
            Call align.Add(ion)

            class_id = ClusterTree.Add(clustering, args.SetTargetKey(ion.lib_guid))
            clusters.Add(class_id)
        Next

        Return New TreeCluster With {
            .tree = clustering,
            .spectrum = ionsList.ToArray,
            .clusters = clusters.ToArray
        }
    End Function

    Public Function Tree([continue] As TreeCluster,
                         ions As IEnumerable(Of PeakMs2),
                         <Out>
                         Optional ByRef clusters As String() = Nothing) As TreeCluster

        Dim ionsList As New List(Of PeakMs2)
        Dim clustering As ClusterTree = [continue].tree
        Dim classes As New List(Of String)
        Dim class_id As String
        Dim args As New ClusterTree.Argument With {
            .alignment = align,
            .threshold = equals_cutoff,
            .diff = diff
        }

        For Each ion As PeakMs2 In ions.SafeQuery
            Call ionsList.Add(ion)
            Call align.Add(ion)

            class_id = ClusterTree.Add(clustering, args.SetTargetKey(ion.lib_guid))
            classes.Add(class_id)
        Next

        clusters = classes.ToArray

        Return New TreeCluster With {
            .tree = clustering,
            .spectrum = [continue].spectrum _
                .JoinIterates(ionsList) _
                .ToArray,
            .clusters = [continue].clusters _
                .JoinIterates(classes) _
                .ToArray
        }
    End Function
End Class
