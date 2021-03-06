﻿using AssetViewer.Templates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetViewer.Data.Filters {

  public class AvailableFilter : BaseFilter<bool> {

    #region Properties

    public override Func<IQueryable<TemplateAsset>, IQueryable<TemplateAsset>> FilterFunc => result => {
      if (SelectedValue)
        result = result.Where(w => (w.Sources != null ? w.Sources.Count : 0) > 0);
      return result;
    };

    public override string DescriptionID => "-1101";
    public override IEnumerable<bool> ComparisonValues => base.ComparisonValues;

    #endregion Properties

    #region Constructors

    public AvailableFilter(ItemsHolder itemsHolder) : base(itemsHolder) {
      FilterType = FilterType.Bool;
    }

    #endregion Constructors

    #region Methods

    public override void ResetFilter() {
      SelectedValue = true;
    }

    #endregion Methods
  }
}