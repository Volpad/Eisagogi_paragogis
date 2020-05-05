using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Eisagogi_paragogis
{
    class DocumentPaginatorImpl : DocumentPaginator
    {
        private List<UIElement> Pages { get; set; }

        public DocumentPaginatorImpl(List<UIElement> pages)
        {
            Pages = pages;
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            return new DocumentPage(Pages[pageNumber]);
        }

        public override bool IsPageCountValid
        {
            get { return true; }
        }

        public override int PageCount
        {
            get { return Pages.Count; }
        }

        public override System.Windows.Size PageSize
        {
            get
            {
                /* Assume the first page is the size of all the pages, for simplicity. */
                if (Pages.Count > 0)
                {
                    UIElement page = Pages[0];

                    if (page is Canvas)
                        return new Size(((Canvas)page).Width, ((Canvas)page).Height);
                    // else if ...
                }

                return Size.Empty;
            }
            set
            {
                /* Ignore the PageSize suggestion. */
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }
    }
}
