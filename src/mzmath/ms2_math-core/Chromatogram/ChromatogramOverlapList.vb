﻿#Region "Microsoft.VisualBasic::64a41b6f3431c042298713172bb9a251, mzkit\src\assembly\mzPack\Stream\ChromatogramOverlap.vb"

' Author:
' 
'       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
' 
' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
' 
' 
' MIT License
' 
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 104
'    Code Lines: 77
' Comment Lines: 10
'   Blank Lines: 17
'     File Size: 3.60 KB


' Class ChromatogramOverlap
' 
'     Properties: length, overlaps
' 
'     Function: EnumerateSignals, (+2 Overloads) getByName, getNames, hasName, (+2 Overloads) setByName
'               setNames, ToString
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Chromatogram

    ''' <summary>
    ''' a collection of <see cref="Chromatogram"/>
    ''' </summary>
    Public Class ChromatogramOverlapList

        Public Property overlaps As New Dictionary(Of String, Chromatogram)

        Default Public Overloads Property TIC(refName As String) As Chromatogram
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return _overlaps(refName)
            End Get
            Set
                _overlaps(refName) = Value
            End Set
        End Property

        Default Public Overloads ReadOnly Property TIC(refNames As String()) As ChromatogramOverlapList
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return New ChromatogramOverlapList With {
                    .overlaps = refNames _
                        .ToDictionary(Function(name) name,
                                      Function(name)
                                          Return _overlaps(name)
                                      End Function)
                }
            End Get
        End Property

        Public ReadOnly Property length As Integer
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return overlaps.Count
            End Get
        End Property

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return overlaps.Keys.ToArray.GetJson
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Iterator Function EnumerateSignals() As IEnumerable(Of NamedValue(Of Chromatogram))
            For Each item In overlaps
                Yield New NamedValue(Of Chromatogram) With {
                    .Name = item.Key,
                    .Value = item.Value
                }
            Next
        End Function

    End Class
End Namespace