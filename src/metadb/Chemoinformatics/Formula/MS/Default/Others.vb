﻿#Region "Microsoft.VisualBasic::c5c6a6b5a50c86122ae20c8afd573753, mzkit\src\metadb\FormulaSearch.Extensions\AtomGroups\Default\Others.vb"

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

    '   Total Lines: 14
    '    Code Lines: 10
    ' Comment Lines: 0
    '   Blank Lines: 4
    '     File Size: 609 B


    '     Class Others
    ' 
    '         Properties: CH2O, H, nitro_group, NL2H2O, NLH2O
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.BioDeep.Chemoinformatics.Formula

Namespace Formula.MS.AtomGroups

    Public Class Others

        Public Shared ReadOnly Property H As Formula = FormulaScanner.ScanFormula("H")
        Public Shared ReadOnly Property nitro_group As Formula = FormulaScanner.ScanFormula("NO2")
        Public Shared ReadOnly Property NLH2O As Formula = FormulaScanner.ScanFormula("H2O")
        Public Shared ReadOnly Property NL2H2O As Formula = FormulaScanner.ScanFormula("(H2O)2")
        Public Shared ReadOnly Property CH2O As Formula = FormulaScanner.ScanFormula("CH2O")

    End Class
End Namespace
