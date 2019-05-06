﻿using System;
using System.Xml.Linq;

namespace AssetViewer.Data {

  public class Icon {

    #region Properties
    public String Filename { get; set; }
    #endregion

    #region Constructor
    public Icon(XElement item) {
      this.Filename = $"/AssetViewer;component/Resources/{item.Element("Filename").Value}";
    }
    #endregion

  }

}