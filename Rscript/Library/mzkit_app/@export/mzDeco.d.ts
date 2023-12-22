﻿// export R# package module type define for javascript/typescript language
//
//    imports "mzDeco" from "mz_quantify";
//
// ref=mzkit.mzDeco@mz_quantify, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * Extract peak and signal data from rawdata
 * 
*/
declare namespace mzDeco {
   module mz {
      /**
       * do ``m/z`` grouping under the given tolerance
       * 
       * > the ion mz value is generated via the max intensity point in each ion 
       * >  feature group, and the xic data has already been re-order via the 
       * >  time asc.
       * 
        * @param ms1 a LCMS mzpack rawdata object or a collection of the ms1 point data
        * @param mzdiff the mass tolerance error for extract the XIC from the rawdata set
        * 
        * + default value Is ``'ppm:20'``.
        * @param rtwin the rt tolerance window size for merge data points
        * 
        * + default value Is ``0.05``.
        * @param env -
        * 
        * + default value Is ``null``.
        * @return create a list of XIC dataset for run downstream deconv operation
      */
      function groups(ms1: any, mzdiff?: any, rtwin?: number, env?: object): object;
   }
   /**
    * Chromatogram data deconvolution
    * 
    * 
     * @param ms1 a collection of the ms1 data or the mzpack raw data object, this parameter could also be
     *  a XIC pool object which contains a collection of the ion XIC data for run deconvolution.
     * @param tolerance the mass tolerance for extract the XIC data for run deconvolution.
     * 
     * + default value Is ``'ppm:20'``.
     * @param baseline 
     * + default value Is ``0.65``.
     * @param peak_width 
     * + default value Is ``'3,20'``.
     * @param joint 
     * + default value Is ``false``.
     * @param parallel 
     * + default value Is ``false``.
     * @param feature a numeric vector of target feature ion m/z value for extract the XIC data.
     * 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
     * @return a vector of the peak deconvolution data,
     *  in format of xcms peak table liked or mzkit @``T:BioNovoGene.Analytical.MassSpectrometry.Math.PeakFeature``
     *  data object.
   */
   function mz_deco(ms1: any, tolerance?: any, baseline?: number, peak_width?: any, joint?: boolean, parallel?: boolean, feature?: any, env?: object): object|object;
   /**
    * Do COW peak alignment and export peaktable
    *  
    *  Correlation optimized warping (COW) based on the total ion 
    *  current (TIC) is a widely used time alignment algorithm 
    *  (COW-TIC). This approach works successfully on chromatograms 
    *  containing few compounds and having a well-defined TIC.
    * 
    * 
     * @param samples should be a set of sample file data, which could be extract from the ``mz_deco`` function.
     * @param mzdiff -
     * 
     * + default value Is ``'da:0.001'``.
     * @param norm do total ion sum normalization after peak alignment and the peaktable object has been exported?
     * 
     * + default value Is ``false``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function peak_alignment(samples: any, mzdiff?: any, norm?: boolean, env?: object): object;
   /**
    * debug used only
    * 
    * 
     * @param pool -
     * @param mz -
     * @param dtw -
     * 
     * + default value Is ``true``.
     * @param mzdiff -
     * 
     * + default value Is ``0.01``.
   */
   function pull_xic(pool: object, mz: number, dtw?: boolean, mzdiff?: number): any;
   module read {
      /**
       * read the peak feature table data
       * 
       * 
        * @param file -
        * @param readBin does the given data file is in binary format not a csv table file, 
        *  and this function should be parsed as a binary data file?
        * 
        * + default value Is ``false``.
      */
      function peakFeatures(file: string, readBin?: boolean): object;
   }
   module write {
      /**
       * write peak debug data
       * 
       * 
        * @param peaks -
        * @param file -
        * @param env -
        * 
        * + default value Is ``null``.
      */
      function peaks(peaks: any, file: any, env?: object): any;
   }
   /**
   */
   function xic_pool(files: string): object;
}
