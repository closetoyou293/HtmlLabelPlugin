﻿using System;
using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Java.Util;
using Plugin.HtmlLabel;
using Plugin.HtmlLabel.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace Plugin.HtmlLabel.Android
{
    public class HtmlLabelRenderer : LabelRenderer
    {
        public HtmlLabelRenderer(Context context) : base(context)
        {
        }

        public static void Initialize() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;
            UpdateText();
            UpdateMaxLines();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == HtmlLabel.MaxLinesProperty.PropertyName)
                UpdateMaxLines();
            else if (e.PropertyName == Label.TextProperty.PropertyName ||
                     e.PropertyName == HtmlLabel.IsHtmlProperty.PropertyName ||
                     e.PropertyName == HtmlLabel.RemoveHtmlTagsProperty.PropertyName ||
                     e.PropertyName == Label.FontAttributesProperty.PropertyName ||
                     e.PropertyName == Label.FontFamilyProperty.PropertyName ||
                     e.PropertyName == Label.FontSizeProperty.PropertyName ||
                     e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName ||
                     e.PropertyName == Label.TextColorProperty.PropertyName)
                UpdateText();
        }

        private void UpdateMaxLines()
        {
            var maxLines = HtmlLabel.GetMaxLines(Element);
            if (maxLines == default(int)) return;
            Control.SetMaxLines(maxLines);
        }

        private void UpdateText()
        {
            if (Control == null || Element == null) return;

            var isHtml = HtmlLabel.GetIsHtml(Element);
            if (!isHtml) return;
            
            var removeTags = HtmlLabel.GetRemoveHtmlTags(Element);

            var text = removeTags ?
                HtmlToText.ConvertHtml(Control.Text) :
                Element.Text;

            var helper = new LabelRendererHelper(Element, text);
            
            var value = helper.ToString();
            var html = Build.VERSION.SdkInt >= BuildVersionCodes.N ?
                Html.FromHtml(value, FromHtmlOptions.ModeCompact) :
                Html.FromHtml(value);
            Control.SetText(html, TextView.BufferType.Spannable);
            Control.MovementMethod = LinkMovementMethod.Instance;
        }
    }
}