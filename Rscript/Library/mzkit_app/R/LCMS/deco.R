imports "mzDeco" from "mz_quantify";
imports "mzweb" from "mzkit";
imports "Parallel" from "snowFall";
imports ["data","math"] from "mzkit";

#' Do ms1 deconvolution of the rawdata
#' 
#' @param rawdata a directory path that contains the mzXML or mzML rawdata files.
#' @param outputdir a directory path for save the peaktable result file and the
#'     temp cache files.
#' @param mzdiff the mass tolerance error for create the mz bins from the ms1 
#'     rawdata, the XIC data is generated based on this parameter value
#' @param peak.width the rt range of the peak data
#' 
#' @return this function returns nothing 
#' 
const run.Deconvolution = function(rawdata, outputdir = "./", mzdiff = 0.005, 
                                   peak.width = [3, 90]) {
                                    
    const xic_cache = `${outputdir}/XIC_data`;
    const files = list.files(rawdata, pattern = ["*.mzML", "*.mzXML", "*.mzPack"]);

    # create temp data of ms1 XIC
    ms1_xic_bins(files, mzdiff = mzdiff, 
        outputdir = xic_cache, 
        n_threads = 32);
    
    const xic_files = list.files(xic_cache, pattern = "*.xic");
    const bins = ms1_mz_bins(files = xic_files);

    write.csv(bins, file = `${outputdir}/mzbins.csv`, 
        row.names = FALSE);

    const peaktable = ms1_peaktable(xic_files, bins, 
        mzdiff = mzdiff, 
        peak.width = peak.width);

    write.csv(peaktable, file = `${outputdir}/peaktable.csv`, 
        row.names = TRUE);

    invisible(NULL);
} 