﻿using System.Reflection;
using Microsoft.AspNetCore.Components.Rendering;

namespace Logistics.Blazor.Grids;

public partial class Column : ComponentBase
{
    private RenderFragment? headerTemplate;
    private RenderFragment<object>? cellTemplate;

    [Parameter]
    public string? Caption { get; set; }

    [Parameter]
    public string? Format { get; set; }

    [Parameter]
    public int? Width { get; set; }

    [CascadingParameter]
    private object? OwnerGrid { get; set; }

    [Parameter]
    public string? Field { get; set; }

    public SortDirection? SortDirection { get; set; }

    [Parameter]
    public RenderFragment<object>? Template { get; set; }

    internal RenderFragment HeaderTemplate => headerTemplate ??= RenderHeader;

    internal RenderFragment<object> CellTemplate => cellTemplate ??= RenderCell;

    protected override void OnInitialized()
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        OwnerGrid?.GetType().GetMethod("AddColumn", flags)?.Invoke(OwnerGrid, new[] { this });
    }

    private void RenderHeader(RenderTreeBuilder builder)
    {
        var caption = Caption;
        if (string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(Field))
        {
            caption = Field;
        }

        builder.AddContent(0, caption);
    }

    private RenderFragment RenderCell(object item)
    {
        return builder =>
        {
            if (!string.IsNullOrEmpty(Field))
            {
                var value = item.GetType().GetProperty(Field)?.GetValue(item);
                var formattedValue = string.IsNullOrEmpty(Format) ? value?.ToString() : string.Format("{0:" + Format + "}", value);
                builder.AddContent(0, formattedValue);
            }
            else
            {
                builder.AddContent(1, Template, item);
            }
        };
    }
}