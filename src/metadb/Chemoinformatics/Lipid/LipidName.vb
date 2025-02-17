﻿#Region "Microsoft.VisualBasic::45ea2585ecad38bd32437f8da638b085, mzkit\src\metadb\Chemoinformatics\Lipid\LipidName.vb"

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

    '   Total Lines: 79
    '    Code Lines: 55
    ' Comment Lines: 9
    '   Blank Lines: 15
    '     File Size: 2.54 KB


    ' Class LipidName
    ' 
    '     Properties: chains, className, hasStructureInfo
    ' 
    '     Function: ChainParser, ParseLipidName, ToOverviewName, ToString, ToSystematicName
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' the lipid name data
''' </summary>
''' <remarks>
''' the function <see cref="ParseLipidName"/> could be used for parse the lipid name string into 
''' the class name with the multiple <see cref="Chain"/> information. the lipid name that we parsed
''' should not be a common name.
''' </remarks>
Public Class LipidName

    ''' <summary>
    ''' the main class of current lipid metabolite
    ''' </summary>
    ''' <returns></returns>
    Public Property className As String
    ''' <summary>
    ''' the carbon chains of current lipid data
    ''' </summary>
    ''' <returns></returns>
    Public Property chains As Chain()

    Public ReadOnly Property hasStructureInfo As Boolean
        Get
            Return chains.All(Function(c) c.hasStructureInfo)
        End Get
    End Property

    Public Overrides Function ToString() As String
        If chains.Length = 1 AndAlso Not chains(Scan0).hasStructureInfo Then
            Return ToOverviewName()
        Else
            Return ToSystematicName()
        End If
    End Function

    Public Function ToSystematicName() As String
        Return $"{className}({(From chain As Chain
                               In chains
                               Let str = chain.ToString
                               Select str).JoinBy("_")})"
    End Function

    ''' <summary>
    ''' $"{className}({totalCarbons}:{totalDBes})"
    ''' </summary>
    ''' <returns></returns>
    Public Function ToOverviewName() As String
        Dim totalCarbons As Integer = Aggregate c As Chain In chains Into Sum(c.carbons)
        Dim totalDBes As Integer = Aggregate c As Chain In chains Into Sum(c.doubleBonds)

        Return $"{className}({totalCarbons}:{totalDBes})"
    End Function

    ''' <summary>
    ''' parse lipid name components
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    Public Shared Function ParseLipidName(name As String) As LipidName
        Static namePattern1 As New Regex("[a-zA-Z0-9]+\(.+\)")
        Static namePattern2 As New Regex("[a-zA-Z0-9]+\s+.+")

        Dim className As String
        Dim components As String

        If name.IsPattern(namePattern1) Then
            Dim tokens = name.GetTagValue("(", trim:=True)

            className = tokens.Name
            components = tokens.Value
            components = components.Substring(0, components.Length - 1)
        Else
            Dim tokens = name.GetTagValue(" ", trim:=True)

            className = tokens.Name
            components = tokens.Value.Trim
        End If

        Return New LipidName With {
            .className = className,
            .chains = ChainParser(components).ToArray
        }
    End Function

    Private Shared Iterator Function ChainParser(components As String) As IEnumerable(Of Chain)
        Dim parts As String() = components.StringSplit("[/_]")

        For Each components In parts
            Yield Chain.ParseName(components)
        Next
    End Function
End Class
