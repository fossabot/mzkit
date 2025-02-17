﻿Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.Xml
Imports Microsoft.VisualBasic.DataMining.BinaryTree

''' <summary>
''' A score evaluator that contains the cache
''' </summary>
''' <remarks>
''' this module has a <see cref="cache"/> for get spectrum data by a unique reference id
''' </remarks>
Public Class MSScoreGenerator : Inherits ComparisonProvider

    ReadOnly getSpectrum As Func(Of String, PeakMs2)
    ReadOnly cache As New Dictionary(Of String, PeakMs2)

    Protected ReadOnly align As AlignmentProvider

    Public ReadOnly Property Ions As IEnumerable(Of PeakMs2)
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return cache.Values
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="align"></param>
    ''' <param name="getSpectrum">
    ''' get spectrum data by unique reference id[cache system was implemented in this module]
    ''' </param>
    ''' <param name="equals"></param>
    ''' <param name="gt"></param>
    Sub New(align As AlignmentProvider, getSpectrum As Func(Of String, PeakMs2),
            Optional equals As Double = 1,
            Optional gt As Double = 0)
        Call MyBase.New(equals, gt)

        Me.align = align
        Me.getSpectrum = getSpectrum
    End Sub

    Sub New(align As AlignmentProvider, Optional equals As Double = 1, Optional gt As Double = 0)
        Call MyBase.New(equals, gt)

        Me.align = align
        Me.getSpectrum = Function(guid) Nothing
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="align"></param>
    ''' <param name="ions">
    ''' these source spectrum data collection for run the similarity evaluation
    ''' </param>
    ''' <param name="equals"></param>
    ''' <param name="gt"></param>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Sub New(align As AlignmentProvider, ions As PeakMs2(), equals As Double, gt As Double)
        Call Me.New(align, AddressOf ions.ToDictionary(Function(i) i.lib_guid).GetValueOrNull, equals, gt)

        For Each spec As PeakMs2 In ions
            Call Add(spec)
        Next
    End Sub

    ''' <summary>
    ''' clear the cache of the spectrum data pool
    ''' </summary>
    Public Sub Clear()
        Call cache.Clear()
    End Sub

    Public Sub Add(spectral As PeakMs2)
        If Not cache.ContainsKey(spectral.lib_guid) Then
            Call cache.Add(spectral.lib_guid, spectral)
        End If
    End Sub

    ''' <summary>
    ''' get spectrum from dictionary via a key
    ''' </summary>
    ''' <param name="guid"></param>
    ''' <returns></returns>
    Public Function GetSpectral(guid As String) As PeakMs2
        If Not cache.ContainsKey(guid) Then
            cache.Add(guid, getSpectrum(guid))
        End If

        Return cache(guid)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetAlignment(x As String, y As String) As AlignmentOutput
        Return align.CreateAlignment(GetSpectral(x).mzInto, GetSpectral(y).mzInto)
    End Function

    ''' <summary>
    ''' get the spectrum similarity score via theirs unique reference id
    ''' </summary>
    ''' <param name="x">
    ''' the <see cref="PeakMs2.lib_guid"/> unique reference id of spectrum object x
    ''' </param>
    ''' <param name="y">
    ''' the <see cref="PeakMs2.lib_guid"/> unique reference id of spectrum object y
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' spectrum could be <see cref="Add"/>
    ''' </remarks>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function GetSimilarity(x As String, y As String) As Double
        Return align.GetScore(GetSpectral(x).mzInto, GetSpectral(y).mzInto)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return $"[{Me.GetHashCode.ToHexString}] {align.ToString}, has {cache.Count} spectrum data cached."
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function GetObject(id As String) As Object
        Return GetSpectral(guid:=id)
    End Function
End Class
