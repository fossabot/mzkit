﻿#Region "Microsoft.VisualBasic::4f2295f1c03027b8274d7fe5fd2016fd, mzkit\src\assembly\NMRFidTool\Math\baseline\BaselineCorrector.vb"

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

    '   Total Lines: 35
    '    Code Lines: 5
    ' Comment Lines: 26
    '   Blank Lines: 4
    '     File Size: 1.17 KB


    '     Interface BaselineCorrector
    ' 
    '         Function: correctBaseline
    ' 
    ' 
    ' /********************************************************************************/

#End Region

' 
'  Copyright (c) 2013 EMBL, European Bioinformatics Institute.
' 
'  This program is free software: you can redistribute it and/or modify
'  it under the terms of the GNU Lesser General Public License as published by
'  the Free Software Foundation, either version 3 of the License, or
'  (at your option) any later version.
' 
'  This program is distributed in the hope that it will be useful,
'  but WITHOUT ANY WARRANTY; without even the implied warranty of
'  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'  GNU Lesser General Public License for more details.
' 
'  You should have received a copy of the GNU Lesser General Public License
'  along with this program.  If not, see <http://www.gnu.org/licenses/>.
' 

Namespace fidMath.Baseline

    ''' <summary>
    ''' Class for baseline correction
    ''' 
    ''' @author  Luis F. de Figueiredo
    ''' 
    ''' User: ldpf
    ''' Date: 29/05/2013
    ''' Time: 15:38
    ''' To change this template use File | Settings | File Templates.
    ''' </summary>
    Public Interface BaselineCorrector

        Function correctBaseline(spectrum As Spectrum) As Spectrum
    End Interface

End Namespace
