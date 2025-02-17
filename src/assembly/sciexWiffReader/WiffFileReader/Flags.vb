﻿#Region "Microsoft.VisualBasic::2a3a302f88eb0b215595a863e32b6e86, mzkit\src\assembly\sciexWiffReader\WiffFileReader\Flags.vb"

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

    '   Total Lines: 17
    '    Code Lines: 14
    ' Comment Lines: 0
    '   Blank Lines: 3
    '     File Size: 298 B


    ' Enum ScanMode
    ' 
    '     Centroid, Profile
    ' 
    '  
    ' 
    ' 
    ' 
    ' Enum ScanType
    ' 
    '     MS1, MS2, Unknown
    ' 
    '  
    ' 
    ' 
    ' 
    ' Enum Polarity
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Clearcore2.Data.DataAccess.SampleData.MSExperimentInfo

Public Enum ScanMode
    Profile
    Centroid
End Enum

Public Enum ScanType
    MS1
    MS2
    Unknown
End Enum

Public Enum Polarity
    Positive = PolarityEnum.Positive
    Negative = PolarityEnum.Negative
End Enum
