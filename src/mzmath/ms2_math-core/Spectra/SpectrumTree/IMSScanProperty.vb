﻿Imports BioNovoGene.Analytical.MassSpectrometry.Math.Chromatogram
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1.PrecursorType

Namespace Spectra

    ''' <summary>
    ''' A spectrum object
    ''' </summary>
    Public Interface IMSScanProperty
        Inherits IMSProperty
        Property ScanID As Integer
        Property Spectrum As List(Of SpectrumPeak)
        Sub AddPeak(mass As Double, intensity As Double, Optional comment As String = Nothing)
    End Interface

    Public Interface IMSProperty
        Property ChromXs As ChromXs
        Property IonMode As IonModes
        Property PrecursorMz As Double
    End Interface
End Namespace