﻿#Region "Microsoft.VisualBasic::9304093281114eca65544b5a11a034a3, mzkit\src\metadb\Chemoinformatics\MetaboliteAnnotation.vb"

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

    '   Total Lines: 16
    '    Code Lines: 12
    ' Comment Lines: 0
    '   Blank Lines: 4
    '     File Size: 690 B


    ' Class MetaboliteAnnotation
    ' 
    '     Properties: CommonName, ExactMass, Formula, UniqueId
    ' 
    '     Function: ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1.Annotations
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic

''' <summary>
''' A basic metabolite annotation data model
''' </summary>
''' <remarks>
''' contains the basic annotation metadata: id, name, exact mass, and formula data
''' </remarks>
Public Class MetaboliteAnnotation
    Implements GenericCompound
    Implements IReadOnlyId, ICompoundNameProvider, IExactMassProvider, IFormulaProvider

    ''' <summary>
    ''' the unique reference id of current metabolite
    ''' </summary>
    ''' <returns></returns>
    Public Property Id As String Implements IReadOnlyId.Identity
    ''' <summary>
    ''' common name of the current metabolite
    ''' </summary>
    ''' <returns></returns>
    Public Property CommonName As String Implements ICompoundNameProvider.CommonName
    ''' <summary>
    ''' exact mass value of current metabolite, which it could be evaluated from the <see cref="Formula"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property ExactMass As Double Implements IExactMassProvider.ExactMass
    ''' <summary>
    ''' atom compositions of current metabolite
    ''' </summary>
    ''' <returns></returns>
    Public Property Formula As String Implements IFormulaProvider.Formula

    Public Overrides Function ToString() As String
        Return $"[{Id}] {CommonName}"
    End Function

End Class
