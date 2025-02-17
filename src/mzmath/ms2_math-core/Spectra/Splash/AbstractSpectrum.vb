﻿'
'  AbstractSpectrum.cs
'
'  Author:
'       Diego Pedrosa <dpedrosa@ucdavis.edu>
'
'  Copyright (c) 2015 Diego Pedrosa
'
'  This library is free software; you can redistribute it and/or modify
'  it under the terms of the GNU Lesser General Public License as
'  published by the Free Software Foundation; either version 2.1 of the
'  License, or (at your option) any later version.
'
'  This library is distributed in the hope that it will be useful, but
'  WITHOUT ANY WARRANTY; without even the implied warranty of
'  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
'  Lesser General Public License for more details.
'
'  You should have received a copy of the GNU Lesser General Public
'  License along with this library; if not, write to the Free Software
'  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

Imports System.Runtime.CompilerServices

Namespace Spectra.SplashID

    Public Module AbstractSpectrum

        Public Const MAX_RELATIVE_INTENSITY As Integer = 100

        ReadOnly splashMs As New Splash(SpectrumType.MS)

        <Extension>
        Public Function MsSplashId(Of T As {New, ISpectrum})(spec As T) As String
            Return splashMs.CalcSplashID(spec)
        End Function

        ''' <summary>
        ''' returns the current spectrum's list of ions sorted by descending intensities
        ''' </summary>
        ''' <param name="desc"></param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function getSortedIonsByIntensity(spec As ISpectrum, Optional desc As Boolean = True) As List(Of ms2)
            Dim sorted As List(Of ms2) = spec.GetIons.OrderByDescending(Function(i) i.intensity).ThenBy(Function(m) m.mz).ToList()

            If Not desc Then
                sorted.Reverse()
            End If

            Return sorted
        End Function

        ''' <summary>
        ''' returns the current spectrum's list of ions sorted by ascending m/z ratio values
        ''' </summary>
        ''' <param name="desc"></param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function getSortedIonsByMZ(spec As ISpectrum, Optional desc As Boolean = False) As List(Of ms2)
            Dim sorted As List(Of ms2) = spec.GetIons.OrderBy(Function(i) i.mz).ThenByDescending(Function(i) i.intensity).ToList()

            If desc Then
                sorted.Reverse()
            End If

            Return sorted
        End Function

        ''' <summary>
        ''' calculate the relative spectrum in the range [0 .. 100]
        ''' </summary>
        ''' <param name="scale"></param>
        ''' <returns></returns>
        ''' 
        <Extension>
        Public Function toRelative(spec As ISpectrum, Optional scale As Double = MAX_RELATIVE_INTENSITY) As List(Of ms2)
            Dim relativeIons As New List(Of ms2)(spec.GetIons)
            Dim maxInt = relativeIons.Max(Function(ion) ion.intensity)

            Call relativeIons _
                .ForEach(Sub(i)
                             i.intensity = i.intensity / maxInt * scale
                         End Sub)

            Return relativeIons
        End Function

    End Module
End Namespace
