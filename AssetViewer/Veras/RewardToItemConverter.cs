﻿using AssetViewer.Library;
using AssetViewer.Templates;
using AssetViewer.Veras;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AssetViewer.Converter {

    [ValueConversion(typeof(IEnumerable<ExpeditionEventPathRewardsItem>), typeof(IEnumerable<TemplateAsset>))]
    [ValueConversion(typeof(ExpeditionEventPathRewardsItem), typeof(IEnumerable<TemplateAsset>))]
    public class RewardToItemConverter : IValueConverter {

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is ExpeditionEventPathRewardsItem item) {
                if (App.Language == Languages.German) {
                    return item.ID.GetItemsById().OrderBy(l => l.Text.DE);
                }
                else {
                    return item.ID.GetItemsById().OrderBy(l => l.Text.EN);
                }
            }
            else if (value is IEnumerable<ExpeditionEventPathRewardsItem> rewards) {
                if (App.Language == Languages.German) {
                    return rewards.SelectMany(l => l.ID.GetItemsById().OrderBy(k => k.Text.DE));
                }
                else {
                    return rewards.SelectMany(l => l.ID.GetItemsById().OrderBy(k => k.Text.EN));
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}