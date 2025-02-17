﻿#Region "Microsoft.VisualBasic::c17940d26a8100873a53e007c86e2069, mzkit\src\metadb\Chemoinformatics\NaturalProduct\NameToken.vb"

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

    '   Total Lines: 34
    '    Code Lines: 15
    ' Comment Lines: 15
    '   Blank Lines: 4
    '     File Size: 780 B


    '     Class Token
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '     Enum NameTokens
    ' 
    '         close, na, name, number, open
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace NaturalProduct

    Public Class Token : Inherits CodeToken(Of NameTokens)

        Public Sub New(name As NameTokens, value As String)
            MyBase.New(name, value)
        End Sub
    End Class

    Public Enum NameTokens
        ''' <summary>
        ''' invalid component name scanner token
        ''' </summary>
        na
        ''' <summary>
        ''' glycosyl
        ''' </summary>
        name
        ''' <summary>
        ''' (
        ''' </summary>
        open
        ''' <summary>
        ''' )
        ''' </summary>
        close
        ''' <summary>
        ''' <see cref="QuantityPrefix"/>
        ''' </summary>
        number
    End Enum
End Namespace
