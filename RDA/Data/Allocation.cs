﻿using System;
using System.Xml.Linq;

namespace RDA.Data {

  public class Allocation {

    #region Properties
    public String ID { get; set; }
    public Icon Icon { get; set; }
    public Description Text { get; set; }
    #endregion

    #region Constructor
    public Allocation(String template, String value) {
      switch (template) {
        case "GuildhouseItem":
          switch (value) {
            case "HarborOffice":
              this.ID = value;
              this.Icon = new Icon("data/ui/2kimages/main/3dicons/icon_harbour_kontor.png");
              this.Text = new Description(Helper.GetDescriptionID("HarborOffice"));
              break;
            case "TownHall":
              this.ID = value;
              this.Icon = new Icon("data/ui/2kimages/main/3dicons/icon_townhall.png");
              this.Text = new Description(Helper.GetDescriptionID("TownHall"));
              break;
            case "RadiusBuilding":
              // seems not being implemented, so simply use the TradeUnion
              this.ID = "TradeUnion";
              this.Icon = new Icon("data/ui/2kimages/main/3dicons/icon_guildhouse.png");
              this.Text = new Description("2346");
              break;
            default:
              if (value != null) throw new NotImplementedException();
              this.ID = "TradeUnion";
              this.Icon = new Icon("data/ui/2kimages/main/3dicons/icon_guildhouse.png");
              this.Text = new Description("2346");
              break;
          }
          break;
        case "TownhallItem":
          switch (value) {
            default:
              if (value != null && value != "None") throw new NotImplementedException();
              this.ID = "TownHall";
              this.Icon = new Icon("data/ui/2kimages/main/3dicons/icon_townhall.png");
              this.Text = new Description("2346");
              break;
          }
          break;
        case "HarborOfficeItem":
          switch (value) {
            default:
              if (value != null && value != "None") throw new NotImplementedException();
              this.ID = "HarborOffice";
              this.Icon = new Icon("data/ui/2kimages/main/3dicons/icon_harbour_kontor.png");
              this.Text = new Description(Helper.GetDescriptionID("HarborOffice"));
              break;
          }
          break;
        default:
          throw new NotImplementedException(template);
      }
    }
    #endregion

    #region Public Methods
    public XElement ToXml() {
      var result = new XElement(this.GetType().Name);
      result.Add(new XAttribute("ID", this.ID));
      result.Add(this.Icon.ToXml());
      result.Add(this.Text.ToXml("Text"));
      return result;
    }
    #endregion

  }

}