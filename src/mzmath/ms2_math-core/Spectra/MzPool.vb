﻿Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Algorithm
Imports Microsoft.VisualBasic.Linq
Imports std = System.Math

Namespace Spectra

    ''' <summary>
    ''' represents the ion m/z as a index
    ''' </summary>
    Public Class MzIndex

        Public Property mz As Double

        ''' <summary>
        ''' the index value
        ''' </summary>
        ''' <returns></returns>
        Public Property index As Integer

        Sub New()
        End Sub

        Sub New(mz As Double, Optional index As Integer = 0)
            Me.mz = mz
            Me.index = index
        End Sub

        ''' <summary>
        ''' get the fallback tuple data
        ''' </summary>
        ''' <returns></returns>
        Public Function Tuple() As (mz As Double, Integer)
            Return (mz, index)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return $"[{index}] {mz.ToString}"
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(mzVal As (mz As Double, index As Integer)) As MzIndex
            Return New MzIndex(mzVal.mz, mzVal.index)
        End Operator

        ''' <summary>
        ''' calculate the binary data file offset
        ''' </summary>
        ''' <param name="sizeof"></param>
        ''' <param name="index"></param>
        ''' <returns></returns>
        Public Shared Operator *(sizeof As Integer, index As MzIndex) As Integer
            Return sizeof * index.index
        End Operator

    End Class

    ''' <summary>
    ''' a wrapper of the binary search function for do mz search 
    ''' with given tolerance error in fast speed.
    ''' </summary>
    ''' <remarks>
    ''' this class module is works based on the binary search helper <see cref="BlockSearchFunction"/>
    ''' </remarks>
    Public Class MzPool

        ''' <summary>
        ''' a wrapper of the binary search function
        ''' </summary>
        ReadOnly index As BlockSearchFunction(Of MzIndex)

        ''' <summary>
        ''' the number of the mz peaks input
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property size As Integer
            Get
                Return index.size
            End Get
        End Property

        ''' <summary>
        ''' get a set of the raw input ion mz element values,
        ''' this length of this vector is equals to the <see cref="size"/>
        ''' property value.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property raw As MzIndex()
            Get
                Return index.raw
            End Get
        End Property

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(mz As IEnumerable(Of Double), Optional win_size As Double = 1)
            index = mz.ToArray.CreateMzIndex(win_size)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(spec As IEnumerable(Of ms2), Optional win_size As Double = 1)
            index = spec.CreateMzIndex(win_size)
        End Sub

        ''' <summary>
        ''' get a set of the index hits that matches the given absolute error.
        ''' </summary>
        ''' <param name="mz">target mz value to do search</param>
        ''' <param name="mzdiff"></param>
        ''' <returns>this function returns empty collection if no hits was found.</returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function Search(mz As Double, Optional mzdiff As Double? = Nothing) As IEnumerable(Of MzIndex)
            Return index.Search(New MzIndex(mz), tolerance:=mzdiff)
        End Function

        Public Iterator Function Query(mz As Double, Optional mzdiff As Double? = Nothing) As IEnumerable(Of (Double, Integer))
            For Each hit In index.Search(New MzIndex(mz), tolerance:=mzdiff)
                Yield (hit.mz, hit.index)
            Next
        End Function

        ''' <summary>
        ''' get the index hit with smallest absolute error
        ''' </summary>
        ''' <param name="mz">the target mz numeric value to do search</param>
        ''' <param name="mzdiff"></param>
        ''' <returns>
        ''' this function returns nothing if no hits could be found
        ''' </returns>
        Public Function SearchBest(mz As Double, Optional mzdiff As Double? = Nothing) As MzIndex
            Dim query As MzIndex() = index.Search(New MzIndex(mz), tolerance:=mzdiff).ToArray

            If query.IsNullOrEmpty Then
                Return Nothing
            ElseIf query.Length = 1 Then
                Return query(0)
            Else
                Return query _
                    .OrderBy(Function(mzi) std.Abs(mzi.mz - mz)) _
                    .First
            End If
        End Function

    End Class
End Namespace