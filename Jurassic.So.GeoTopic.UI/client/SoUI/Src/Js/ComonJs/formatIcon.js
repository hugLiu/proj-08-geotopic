import dataset_gif from '../../Img/FileIcon/DataSet.gif';
import doc_gif from '../../Img/FileIcon/doc.gif';
import ico_gif from '../../Img/FileIcon/ico.gif';
import docx_gif from '../../Img/FileIcon/docx.gif';
import file_gif from '../../Img/FileIcon/file.gif';
import gif_gif from '../../Img/FileIcon/gif.gif';
import htm_gif from '../../Img/FileIcon/htm.gif';
import html_gif from '../../Img/FileIcon/html.gif';
import jpg_gif from '../../Img/FileIcon/jpg.gif';
import pdf_gif from '../../Img/FileIcon/pdf.gif';
import png_gif from '../../Img/FileIcon/png.gif';
import ppt_gif from '../../Img/FileIcon/ppt.gif';
import pptx_gif from '../../Img/FileIcon/pptx.gif';
import rar_gif from '../../Img/FileIcon/rar.gif';
import swf_gif from '../../Img/FileIcon/swf.gif';
import tif_gif from '../../Img/FileIcon/tif.gif';
import txt_gif from '../../Img/FileIcon/txt.gif';
import xlsx_gif from '../../Img/FileIcon/xlsx.gif';
import xls_gif from '../../Img/FileIcon/xls.gif';
import bmp_gif from '../../Img/FileIcon/bmp.gif';
import cab_gif from '../../Img/FileIcon/cab.gif';
import refresh_png from '../../Img/Top/refresh.png';
import thumbnail_png from '../../Img/Detail/slt.png';
import saveImg from '../../Img/FileIcon/save.png';
import collectImg from '../../Img/FileIcon/collect.png';

     const  icons = {
    dataset: dataset_gif,
    doc: doc_gif,
    ioc: ico_gif,
    docx: docx_gif,
    file: file_gif,
    gif: gif_gif,
    htm: htm_gif,
    html: html_gif,
    jpg: jpg_gif,
    pdf: pdf_gif,
    png: png_gif,
    ppt: ppt_gif,
    pptx: pptx_gif,
    rar: rar_gif,
    swf: swf_gif,
    tif: tif_gif,
    txt: txt_gif,
    xlsx: xlsx_gif,
    xls: xls_gif,
    bmp: bmp_gif,
    cab: cab_gif,
    refresh:refresh_png,
    thumbnail:thumbnail_png,
    save:saveImg,
    collect:collectImg,
    
}
export  default  function getIcon(str) {
    return    icons[str];// icons[str];
};
