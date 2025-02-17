import mzkit

from mzkit import mzweb
from mzkit import data
from Rstudio import gtk

targets  = gtk::selectFiles(title = "Select a csv table contains target product fragments", filter = "Excel(*.csv)|*.csv", multiple = False)
files    = gtk::selectFiles(title = "Select raw data files for run ms2 filter", filter = "Thermofisher Raw(*.raw)|*.raw|GC-MS(*.cdf)|*.cdf", throwCancel = False, multiple = True) 
savefile = gtk::selectFiles(title = "Select a table file for save result", filter = "Excel(*.xls)|*.xls", forSave = True)

# tolerance value for match ms2 data
mzdiff  = mzkit::tolerance("da", 0.1)
hits    = None
targets = read.csv(targets, row.names = None)

print("view of the ms2 product ion list:")
print(targets, max.print = 6)

if length(files) == 0:
    raise "no raw data file was selected for run data processing!"
else 
    print("processing raw data files:")
    print(basename(files))

if !all(["name", "mz"] in colnames(targets)):
    raise "missing one of the data fields in your target product fragments table: 'name' or 'mz'!"
else
    print("input check success")

def search_product(filepath, mz):
    
    mzpack    = open.mzpack(filepath)
    products  = ms2_peaks(mzpack)
    xcms_id   = make.ROI_names(products, name.chrs = True)
    
    intensity = data::intensity(products, mz = mz, mzdiff = mzdiff)
    
    # just filter out target fragment with high intensity
    i           = (intensity / max(intensity)) > 0
    target_into = intensity[i]    
    products    = products[i]
    xcms_id     = xcms_id[i]
    mz          = sapply(products, ms2 -> [ms2]::mz)
    rt          = sapply(products, ms2 -> [ms2]::rt)
    rt_min      = round(rt / 60, 2)
    intensity   = sapply(products, ms2 -> [ms2]::intensity)
    totalIons   = sapply(products, ms2 -> [ms2]::Ms2Intensity)
    scan        = sapply(products, ms2 -> [ms2]::scan)
    nsize       = sapply(products, ms2 -> [ms2]::fragments)
    
    topIons   = lapply(products, ms2 -> getTopIons([ms2]::mzInto))
    basePeak  = sapply(topIons, ms2 -> basePeak_toString(ms2[1]))
    top2      = sapply(topIons, ms2 -> mz2_toString(ms2, 2))
    top3      = sapply(topIons, ms2 -> mz2_toString(ms2, 3))
    top4      = sapply(topIons, ms2 -> mz2_toString(ms2, 4))
    top5      = sapply(topIons, ms2 -> mz2_toString(ms2, 5))

    return (xcms_id, mz, rt, rt_min, intensity, totalIons, scan, nsize, basePeak, top2, top3, top4, top5, target_into)

def mz2_toString(ms2, i):
    into = sapply(ms2, x -> [x]::intensity)
    into = round(into / max(into) * 100, 2)
    mz2  = ms2[i]
    
    if is.null(mz2):
        return ""
    else 
        return `${toString([mz2]::mz, format = "F4")}:${into[i]}`

def basePeak_toString(mz2):
    return `${toString([mz2]::mz, format = "F4")}:${toString([mz2]::intensity, format = "G3")}`    

def getTopIons(ms2):
    into = sapply(ms2, x -> [x]::intensity)
    i    = order(into, decreasing = True)
    
    return ms2[i]
    
# loop through all raw data files
for filepath in files:
    
    print(`processing data [${filepath}]...`)
       
    # loop through all target ms2 product ions
    for(mz2 in as.list(targets, byrow = True)):
        
        str(mz2)   
        
        # name, mz   
        name   = mz2[["name"]]
        mzData = mz2[["mz"]]     
                 
        peaks = search_product(filepath, mzData)
        peaks = data.frame(peaks)
        
        print(`get ${nrow(peaks)} ms2 products:`)
        print(peaks, max.print = 6)       
       
        peaks[, "target_mz"]   = mzData
        peaks[, "target_name"] = name        
        peaks[, "samplefile"]  = basename(filepath)
        
        hits = rbind(hits, peaks)
       
rownames(hits) = hits[, "xcms_id"]
       
print(" ~~job done!")
print(`save result file at location: '${savefile}'!`)

print(hits, max.print = 13)

write.csv(hits, file = savefile, row.names = False)
