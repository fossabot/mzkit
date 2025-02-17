﻿Imports System.Runtime.CompilerServices
Imports BioNovoGene.BioDeep.Chemistry.MetaLib.CrossReference
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text.Parser
Imports Microsoft.VisualBasic.Text.Xml.Models
Imports SMRUCC.genomics.foundation.OBO_Foundry.IO.Models
Imports metadata = BioNovoGene.BioDeep.Chemistry.MetaLib.Models.MetaInfo

Namespace ChEBI

    Public Module ChEBIObo

        Public Iterator Function ImportsMetabolites(chebi As OBOFile) As IEnumerable(Of metadata)
            For Each term As RawTerm In chebi.GetRawTerms
                Yield term.ExtractTerm
            Next
        End Function

        <Extension>
        Public Function ExtractTerm(term As RawTerm) As metadata
            Dim obo_data = term.GetValueSet
            Dim id As String = obo_data(RawTerm.Key_id).First
            Dim name As String = obo_data(RawTerm.Key_name).First
            Dim def As String = obo_data(RawTerm.Key_def).JoinBy("; ")
            Dim synonym As String() = obo_data(RawTerm.Key_synonym) _
                .SafeQuery _
                .Select(Function(si) DelimiterParser.GetTokens(si).First) _
                .ToArray
            Dim properties = obo_data(RawTerm.Key_property_value).ParsePropertyValues
            Dim xref = obo_data(RawTerm.Key_xref).ParseXref

            Return New metadata With {
                .description = Strings.Trim(def).Trim(""""c, " "c),
                .ID = id,
                .name = name,
                .synonym = synonym,
                .IUPACName = name,
                .formula = properties.SafeGetString("http://purl.obolibrary.org/obo/chebi/formula"),
                .exact_mass = FormulaScanner.EvaluateExactMass(.formula),
                .xref = New xref With {
                    .CAS = xref.TryGetValue("CAS"),
                    .chebi = id,
                    .KEGG = xref.TryGetValue("KEGG").JoinBy(", "),
                    .InChI = properties.SafeGetString("http://purl.obolibrary.org/obo/chebi/inchi"),
                    .InChIkey = properties.SafeGetString("http://purl.obolibrary.org/obo/chebi/inchikey"),
                    .SMILES = properties.SafeGetString("http://purl.obolibrary.org/obo/chebi/smiles"),
                    .MetaCyc = xref.TryGetValue("MetaCyc").JoinBy(", "),
                    .DrugBank = xref.TryGetValue("DrugBank").JoinBy(", "),
                    .Wikipedia = xref.TryGetValue("Wikipedia").JoinBy(", "),
                    .HMDB = xref.TryGetValue("HMDB").JoinBy(", "),
                    .KNApSAcK = xref.TryGetValue("KNApSAcK").JoinBy(", "),
                    .lipidmaps = xref.TryGetValue("LIPID_MAPS_instance").JoinBy(", ")
                }
            }
        End Function

        <Extension>
        Private Function SafeGetString(properties As Dictionary(Of String, NamedValue()), key As String) As String
            If properties.ContainsKey(key) Then
                Return properties(key).First.text
            Else
                Return Nothing
            End If
        End Function
    End Module
End Namespace