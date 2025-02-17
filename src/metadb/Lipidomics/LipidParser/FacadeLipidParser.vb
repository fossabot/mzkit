﻿Public Class FacadeLipidParser
    Implements ILipidParser
    Private ReadOnly map As New Dictionary(Of String, List(Of ILipidParser))()

    Public ReadOnly Property Target As String = String.Empty Implements ILipidParser.Target

    Public Function Parse(lipidStr As String) As ILipid Implements ILipidParser.Parse
        Dim key = lipidStr.Split()(0)
        Dim parsers As List(Of ILipidParser) = Nothing, lipid As ILipid = Nothing

        If map.TryGetValue(key, parsers) Then
            For Each parser In parsers

                lipid = TryCast(parser.Parse(lipidStr), ILipid)

                If lipid IsNot Nothing Then
                    Return lipid
                End If
            Next
        End If
        Return Nothing
    End Function

    Public Sub Add(parser As ILipidParser)
        If Not map.ContainsKey(parser.Target) Then
            map.Add(parser.Target, New List(Of ILipidParser)())
        End If
        map(parser.Target).Add(parser)
    End Sub

    Public Sub Remove(parser As ILipidParser)
        If map.ContainsKey(parser.Target) Then
            map(parser.Target).Remove(parser)
        End If
    End Sub

    Public Shared ReadOnly Property [Default] As ILipidParser
        Get
            If defaultField Is Nothing Then
                Dim parser = New FacadeLipidParser()
                Call New ILipidParser() {
                        New BMPLipidParser(),
                        New CLLipidParser(),
                        New DGLipidParser(),
                        New HBMPLipidParser(),
                        New LPCLipidParser(),
                        New LPELipidParser(),
                        New LPGLipidParser(),
                        New LPILipidParser(),
                        New LPSLipidParser(),
                        New MGLipidParser(),
                        New PALipidParser(),
                        New PCLipidParser(),
                        New PELipidParser(),
                        New PGLipidParser(),
                        New PILipidParser(),
                        New PSLipidParser(),
                        New TGLipidParser(),
                        New DGLipidParser(),
                        New EtherPCLipidParser(),
                        New EtherPELipidParser(),
                        New EtherLPCLipidParser(),
                        New EtherLPELipidParser(),
                        New SMLipidParser(),
                        New CeramideLipidParser(),
                        New HexCerLipidParser(),
                        New Hex2CerLipidParser(),
                        New DGTALipidParser(),
                        New DGTSLipidParser(),
                        New LDGTALipidParser(),
                        New LDGTSLipidParser(),
                        New GM3LipidParser(),
                        New SHexCerLipidParser(),
                        New CARLipidParser(),
                        New CLLipidParser(),
                        New DMEDFAHFALipidParser(),
                        New DMEDFALipidParser(),
                        New CELipidParser(),
                        New PCd5LipidParser(),
                        New PEd5LipidParser(),
                        New PId5LipidParser(),
                        New PGd5LipidParser(),
                        New PSd5LipidParser(),
                        New LPCd5LipidParser(),
                        New LPEd5LipidParser(),
                        New LPId5LipidParser(),
                        New LPGd5LipidParser(),
                        New LPSd5LipidParser(),
                        New CeramideNsD7LipidParser(),
                        New SMd9LipidParser(),
                        New DGd5LipidParser(),
                        New TGd5LipidParser(),
                        New CEd7LipidParser()
                    }.ForEach(Sub(par, nil) parser.Add(par))
                defaultField = parser
            End If
            Return defaultField
        End Get
    End Property
    Private Shared defaultField As ILipidParser

End Class

