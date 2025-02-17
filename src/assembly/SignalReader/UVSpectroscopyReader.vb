﻿Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.mzML
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.mzData.mzWebCache
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.SignalProcessing
Imports Microsoft.VisualBasic.Text.Xml

Public Module UVSpectroscopyReader

    <Extension>
    Public Function GetSignalModel(UVSpectroscopy As UVScan) As GeneralSignal
        Return New GeneralSignal With {
            .description = UVSpectroscopy.ToString(),
            .Measures = UVSpectroscopy.wavelength,
            .measureUnit = "wavelength",
            .reference = UVSpectroscopy.ToString(),
            .Strength = UVSpectroscopy.intensity,
            .meta = New Dictionary(Of String, String) From {
                {"title", .reference}
            }
        }
    End Function

    <Extension>
    Public Iterator Function GetUVScans(mzml As mzMLScans, instrumentConfigurationId As String) As IEnumerable(Of GeneralSignal)
        For Each scan As spectrum In mzml.GetNoneSpectrumScans
            If Not scan.cvParams.KeyItem(UVScanType) Is Nothing Then
                Yield scan.CreateGeneralSignal(instrumentConfigurationId)
            End If
        Next
    End Function

    ''' <summary>
    ''' electromagnetic radiation spectrum
    ''' </summary>
    ''' <param name="instrumentConfigurationId">
    ''' <see cref="GetPhotodiodeArrayDetectorInstrumentConfigurationId"/>
    ''' </param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Iterator Function PopulatesElectromagneticRadiationSpectrum(spectrums As IEnumerable(Of spectrum), instrumentConfigurationId As String) As IEnumerable(Of GeneralSignal)
        For Each rawScan As spectrum In spectrums
            If rawScan.scanList.scans.Any(Function(scan) scan.instrumentConfigurationRef = instrumentConfigurationId) Then
                Yield rawScan.CreateGeneralSignal(instrumentConfigurationId)
            End If
        Next
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rawScan"></param>
    ''' <param name="instrumentConfigurationId"></param>
    ''' <returns>
    ''' metadata contains:
    ''' 
    ''' 1. total_ion_current
    ''' 2. lowest_wavelength
    ''' 3. highest_wavelength
    ''' 4. scan_time
    ''' 5. rawfile
    ''' 6. scan
    ''' </returns>
    <Extension>
    Public Function CreateGeneralSignal(rawScan As spectrum, instrumentConfigurationId As String) As GeneralSignal
        Dim descriptor As Double() = rawScan.binaryDataArrayList.list(Scan0).Base64Decode
        Dim intensity As Double() = rawScan.binaryDataArrayList.list(1).Base64Decode
        Dim info As New Dictionary(Of String, String)
        Dim UVscan As scan = rawScan.scanList.scans.First(Function(a) a.instrumentConfigurationRef = instrumentConfigurationId)
        Dim title As NamedValue(Of String)() = XmlEntity _
            .UnescapingXmlEntity(rawScan.cvParams.KeyItem("spectrum title")?.value) _
            .Matches("\S+[:]""[^""]+""") _
            .Select(Function(tag) tag.GetTagValue(":")) _
            .ToArray
        Dim file = title.KeyItem("File").Value

        info.Add("total_ion_current", rawScan.cvParams.KeyItem("total ion current")?.value)
        info.Add("lowest_wavelength", rawScan.cvParams.KeyItem("lowest observed wavelength")?.value)
        info.Add("highest_wavelength", rawScan.cvParams.KeyItem("highest observed wavelength")?.value)
        info.Add("scan_time", Val(UVscan.cvParams.KeyItem("scan start time")?.value) * 60)
        info.Add("rawfile", If(file.StringEmpty, "UVraw", file.Trim(""""c)))

        title = rawScan.id.StringSplit("\s+").Select(Function(tag) tag.GetTagValue("=")).ToArray

        info.Add("scan", title.KeyItem("scan").Value)

        Return New GeneralSignal With {
            .description = UVScanType,
            .meta = info,
            .measureUnit = "wavelength(nanometer)",
            .Measures = descriptor,
            .Strength = intensity,
            .reference = $"{info!rawfile}#{info!scan}"
        }
    End Function

    ''' <summary>
    ''' make signal data matrix transpose
    ''' </summary>
    ''' <param name="scans"></param>
    ''' <param name="rawfile"></param>
    ''' <returns></returns>
    <Extension>
    Public Iterator Function CreateTimeSignals(scans As IEnumerable(Of GeneralSignal), Optional rawfile As String = "raw") As IEnumerable(Of GeneralSignal)
        Dim samplers = scans _
            .Select(Function(raw)
                        Return (scan_time:=Val(raw.meta!scan_time), Data:=Resampler.CreateSampler(raw))
                    End Function) _
            .OrderBy(Function(raw) raw.scan_time) _
            .ToArray
        Dim allWavelength As Double() = samplers _
            .Select(Function(a) a.Data.enumerateMeasures) _
            .IteratesALL _
            .Distinct _
            .ToArray
        Dim i As i32 = 1

        For Each wl As Double In allWavelength
            Dim time = samplers.Select(Function(a) a.scan_time).ToArray
            Dim intensity = samplers.Select(Function(a) a.Data.GetIntensity(x:=wl)).ToArray

            Yield New GeneralSignal With {
                .description = "UV ChromatogramTick",
                .Measures = time,
                .measureUnit = "sec",
                .Strength = intensity,
                .reference = ++i,
                .meta = New Dictionary(Of String, String) From {
                    {"wavelength", wl},
                    {"File", rawfile}
                }
            }
        Next
    End Function
End Module
