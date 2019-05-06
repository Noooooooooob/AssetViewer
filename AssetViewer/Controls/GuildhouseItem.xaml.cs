﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using AssetViewer.Library;
using AssetViewer.Templates;

namespace AssetViewer.Controls {

  public partial class GuildhouseItem : UserControl, INotifyPropertyChanged {

    #region Properties
    public IEnumerable<Asset> Items {
      get {
        var rarity = this.ComboBoxRarities.SelectedItem as String;
        var type = this.ComboBoxTypes.SelectedItem as String;
        var target = this.ComboBoxTargets.SelectedItem as String;
        var result = this.Assets;
        if (!String.IsNullOrEmpty(type)) result = result.Where(w => w.ItemType == type);
        switch (App.Language) {
          case Languages.German:
            if (!String.IsNullOrEmpty(rarity)) result = result.Where(w => w.Rarity.DE == rarity);
            if (!String.IsNullOrEmpty(target)) result = result.Where(w => w.EffectTargets.Select(s => s.DE).Contains(target));
            result = result.OrderBy(o => o.Text.DE);
            break;
          default:
            if (!String.IsNullOrEmpty(rarity)) result = result.Where(w => w.Rarity.EN == rarity);
            if (!String.IsNullOrEmpty(target)) result = result.Where(w => w.EffectTargets.Select(s => s.EN).Contains(target));
            result = result.OrderBy(o => o.Text.EN);
            break;
        }
        return result;
      }
    }
    public Asset SelectedAsset { get; set; }
    public LinearGradientBrush RarityBrush {
      get {
        if (this.SelectedAsset == null) return null;
        switch (this.SelectedAsset.Rarity.EN) {
          case "Uncommon":
            return new LinearGradientBrush(new GradientStopCollection() {
              new GradientStop(Color.FromRgb(65, 89, 41), 0),
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);
          case "Rare":
            return new LinearGradientBrush(new GradientStopCollection() {
              new GradientStop(Color.FromRgb(50, 60, 83), 0),
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);
          case "Epic":
            return new LinearGradientBrush(new GradientStopCollection() {
              new GradientStop(Color.FromRgb(90, 65, 89), 0),
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);
          case "Legendary":
            return new LinearGradientBrush(new GradientStopCollection() {
              new GradientStop(Color.FromRgb(98, 66, 46), 0),
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);
          default:
            return new LinearGradientBrush(new GradientStopCollection() {
              new GradientStop(Color.FromRgb(126, 128, 125), 0),
              new GradientStop(Color.FromRgb(42, 44, 39), 0.2),
              new GradientStop(Color.FromRgb(42, 44, 39), 1)
            }, 90);
        }
      }
    }
    public String AllocationText {
      get {
        switch (App.Language) {
          case Languages.German:
            return "Hier ausgerüstet";
          default:
            return "Equipped in";
            break;
        }
      }
    }
    public String ExpeditionText {
      get {
        switch (App.Language) {
          case Languages.German:
            return "Expeditions-Bonus";
          default:
            return "Expedition Bonus";
            break;
        }
      }
    }
    public String TradeText {
      get {
        switch (App.Language) {
          case Languages.German:
            return "Verkaufspreis";
          default:
            return "Selling Price";
            break;
        }
      }
    }
    public IEnumerable<String> Rarities {
      get {
        switch (App.Language) {
          case Languages.German:
            return new String[] {
              String.Empty,
              "Gewöhnlich",
              "Ungewöhnlich",
              "Selten",
              "Episch",
              "Legendär"
            };
          default:
            return new String[] {
              String.Empty,
              "Common",
              "Uncommon",
              "Rare",
              "Epic",
              "Legendary"
            };
        }
      }
    }
    public IEnumerable<String> ItemTypes {
      get {
        var result = this.Assets.Select(s => s.ItemType).Distinct().OrderBy(o => o).ToList();
        result.Insert(0, String.Empty);
        return result;
      }
    }
    public IEnumerable<String> Targets {
      get {
        List<String> result;
        switch (App.Language) {
          case Languages.German:
            result = this.Assets.SelectMany(s => s.EffectTargets).Select(s => s.DE).Distinct().OrderBy(o => o).ToList();
            break;
          default:
            result = this.Assets.SelectMany(s => s.EffectTargets).Select(s => s.EN).Distinct().OrderBy(o => o).ToList();
            break;
        }
        result.Insert(0, String.Empty);
        return result;
      }
    }
    public Boolean HasResult {
      get { return this.Items.Any(); }
    }
    #endregion

    private readonly IEnumerable<Asset> Assets;

    #region Constructor
    public GuildhouseItem() {
      this.InitializeComponent();
      ((MainWindow)Application.Current.MainWindow).ComboBoxLanguage.SelectionChanged += this.ComboBoxLanguage_SelectionChanged;
      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AssetViewer.Resources.Assets.GuildhouseItem.xml")) {
        using (var reader = new StreamReader(stream)) {
          var document = XDocument.Parse(reader.ReadToEnd()).Root;
          this.Assets = document.Elements().Select(s => new Asset(s)).ToArray();
        }
      }
      this.DataContext = this;
    }
    #endregion

    #region Private Methods
    private void GuildhouseItem_OnLoaded(Object sender, RoutedEventArgs e) {
      this.ComboBoxRarities.SelectedIndex = 0;
      this.ComboBoxTypes.SelectedIndex = 0;
      this.ComboBoxTargets.SelectedIndex = 0;
      this.ListBoxItems.SelectedIndex = 0;
      //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
      //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
    }
    private void ComboBoxRarities_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
    }
    private void ComboBoxTypes_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
    }
    private void ComboBoxTargets_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
    }
    //private void CheckBoxFactoryUpgrades_OnChanged(Object sender, RoutedEventArgs e) {
    //  this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
    //  this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
    //  this.ListBoxItems.SelectedIndex = 0;
    //}
    //private void CheckBoxBuildingUpgrades_OnChanged(Object sender, RoutedEventArgs e) {
    //  this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
    //  this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
    //  this.ListBoxItems.SelectedIndex = 0;
    //}
    private void ListBoxItems_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
      if (e.AddedItems.Count == 0) this.ListBoxItems.SelectedIndex = 0;
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedAsset"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RarityBrush"));
    }
    private void ComboBoxLanguage_SelectionChanged(Object sender, SelectionChangedEventArgs e) {
      switch (((ComboBox)sender).SelectedIndex) {
        case 0:
          App.Language = Languages.English;
          break;
        case 1:
          App.Language = Languages.German;
          break;
      }
      //this.ComboBoxAllocations.SelectedItem = null;
      //this.ComboBoxTypes.SelectedItem = null;
      //this.ComboBoxTargets.SelectedItem = null;
      //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Descriptions"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rarities"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ItemTypes"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Targets"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedAsset"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RarityBrush"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllocationText"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExpeditionText"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TradeText"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
      //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Allocations"));
      //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Targets"));
      //this.ComboBoxAllocations.SelectedIndex = 0;
      //this.ComboBoxTypes.SelectedIndex = 0;
      //this.ComboBoxTargets.SelectedIndex = 0;
    }
    private void CheckBoxPopulationUpgrade_OnChanged(Object sender, RoutedEventArgs e) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
      this.ListBoxItems.SelectedIndex = 0;
    }
    private void CheckBoxResidenceUpgrade_OnChanged(Object sender, RoutedEventArgs e) {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Items"));
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasResult"));
      this.ListBoxItems.SelectedIndex = 0;
    }
    #endregion

    public event PropertyChangedEventHandler PropertyChanged;
  }

}