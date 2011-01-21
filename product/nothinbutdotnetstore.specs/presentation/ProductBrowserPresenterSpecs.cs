using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using nothinbutdotnetstore.presentation;
using nothinbutdotnetstore.tasks;
using NUnit.Framework;

namespace nothinbutdotnetstore.specs.presentation
{
    public class ProductBrowserPresenterSpecs
    {
        public class when_initialized_and_there_is_a_department_in_the_payload
        {
            OurProductBrowserView view;
            IEnumerable<Product> results_to_be_returned_by_the_repository;
            OurProductRepository product_repository;
            long department_number;
            NameValueCollection payload;

            [SetUp]
            public void setup()
            {
                view = new OurProductBrowserView();
                results_to_be_returned_by_the_repository = Enumerable.Range(1, 10).Select(x => new Product()).ToList();
                payload = new NameValueCollection();
                department_number = 34;
                product_repository = new OurProductRepository(results_to_be_returned_by_the_repository);

                payload.Add(QueryStrings.DepartmentId,department_number.ToString());

                view.payload = payload;
            }

            [Test]
            public void
                should_tell_the_view_to_display_the_products_retrieved_by_the_products_in_the_department_in_the_payload()
            {
                var sut = new ProductBrowserPresenter(delegate { }, product_repository, view);

                sut.initialize();

                Assert.AreEqual(results_to_be_returned_by_the_repository, view.products);
                Assert.AreEqual(department_number, product_repository.department_requested);
            }
        }

        public class when_initialized_and_there_is_no_payload
        {
            OurProductBrowserView view;
            OurProductRepository product_repository;
            NameValueCollection payload;
            string view_told_to_process;
            string department_view;
            DisplayView display_view_behaviour;

            [SetUp]
            public void setup()
            {
                view = new OurProductBrowserView();
                payload = new NameValueCollection();
                product_repository = new OurProductRepository(null);
                display_view_behaviour = (view_name) => view_told_to_process = view_name;

                view.payload = payload;
            }

            [Test]
            public void
                should_transfer_processing_to_the_department_view()
            {
                new ProductBrowserPresenter(display_view_behaviour, product_repository, view).initialize();

                Assert.AreEqual(Views.Department, view_told_to_process);
            }
        }
    }

    public class OurProductRepository : ProductsRepository
    {
        IEnumerable<Product> products_to_return;
        public int department_requested { get; set; }

        public OurProductRepository(IEnumerable<Product> products_to_return)
        {
            this.products_to_return = products_to_return;
        }

        public IEnumerable<Product> get_products_for_department(int department_id)
        {
            this.department_requested = department_id;
            return products_to_return;
        }
    }

    class OurProductBrowserView : ProductBrowserView
    {
        public NameValueCollection payload { get; set; }
        public IEnumerable<Product> products { get; set; }

        public void display(IEnumerable<Product> products)
        {
            this.products = products;
        }
    }
}