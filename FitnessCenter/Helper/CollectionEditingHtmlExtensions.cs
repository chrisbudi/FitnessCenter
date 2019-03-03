using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Services.Infrastructure
{
    public static class CollectionEditingHtmlExtensions
    {
        private const string JQueryTemplatingEnabledKey = "__BeginCollectionItem_jQuery";

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="collectionName"></param>
        /// <param name="expandedTable"></param>
        /// <param name="modelId"></param>
        /// <param name="expandedCollectionName"></param>
        /// <returns></returns>
        public static IDisposable BeginCollectionItem<TModel>(this HtmlHelper<TModel> html, string collectionName, bool expandedTable = false,
            string modelId = "", string expandedCollectionName = "")
        {
            if (String.IsNullOrEmpty(collectionName))
                throw new ArgumentException("collectionName is null or empty.", "collectionName");

            var collectionIndexFieldName = String.Format("{0}.Index", collectionName);

            string itemIndex = null;
            itemIndex = html.ViewData.ContainsKey(JQueryTemplatingEnabledKey) ? "${index}" : GetCollectionItemIndex(collectionIndexFieldName);
            if (!string.IsNullOrEmpty(modelId))
            {
                itemIndex = modelId;
            }
            string collectionItemName = expandedTable ? $"{collectionName}" : $"{collectionName}[{itemIndex}]";
            var collectionClass = expandedTable ? $"{expandedCollectionName}Index" : $"{collectionName}Index";

            var indexField = new TagBuilder("input");
            indexField.MergeAttributes(new Dictionary<string, string>
            {
                {"name", collectionIndexFieldName},
                {"value", itemIndex},
                {"type", "hidden"},
                {"autocomplete", "off"},
                {"class", collectionClass }
            });

            html.ViewContext.Writer.WriteLine(indexField.ToString(TagRenderMode.SelfClosing));
            return new CollectionItemNamePrefixScope(html.ViewData.TemplateInfo, collectionItemName);
        }




        public static MvcHtmlString CollectionItemJQueryTemplate<TModel, TCollectionItem>(this HtmlHelper<TModel> html,
            string partialViewName,
            TCollectionItem modelDefaultValues)
        {
            var viewData = new ViewDataDictionary<TCollectionItem>(modelDefaultValues);
            viewData.Add(JQueryTemplatingEnabledKey, true);
            return html.Partial(partialViewName, modelDefaultValues, viewData);
        }

        /// <summary>
        ///     Tries to reuse old .Index values from the HttpRequest in order to keep the ModelState consistent
        ///     across requests. If none are left returns a new one.
        /// </summary>
        /// <param name="collectionIndexFieldName"></param>
        /// <returns>a GUID string</returns>
        private static string GetCollectionItemIndex(string collectionIndexFieldName)
        {
            var previousIndices = (Queue<string>)HttpContext.Current.Items[collectionIndexFieldName];
            if (previousIndices == null)
            {
                HttpContext.Current.Items[collectionIndexFieldName] = previousIndices = new Queue<string>();

                var previousIndicesValues = HttpContext.Current.Request[collectionIndexFieldName];
                if (!String.IsNullOrWhiteSpace(previousIndicesValues))
                {
                    foreach (var index in previousIndicesValues.Split(','))
                        previousIndices.Enqueue(index);
                }
            }

            return previousIndices.Count > 0 ? previousIndices.Dequeue() : Guid.NewGuid().GetHashCode().ToString("x");
        }

        private class CollectionItemNamePrefixScope : IDisposable
        {
            private readonly string _previousPrefix;
            private readonly TemplateInfo _templateInfo;

            public CollectionItemNamePrefixScope(TemplateInfo templateInfo, string collectionItemName)
            {
                _templateInfo = templateInfo;

                _previousPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = collectionItemName;
            }

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousPrefix;
            }
        }
    }
}