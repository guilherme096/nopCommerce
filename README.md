﻿nopCommerce: free and open-source eCommerce solution
===========

[nopCommerce](https://www.nopcommerce.com/?utm_source=github&utm_medium=content&utm_campaign=homepage) is the best open-source eCommerce platform. nopCommerce is free, and it is the most popular ASP.NET Core shopping cart.

![nopCommerce demo](https://www.nopcommerce.com/images/github/responsive_devices_codeplex.png#v1)

### Key features ###

* The product is being developed and supported by the professional team since 2008.
* nopCommerce has been downloaded more than 3,000,000 times.
* The active developer community has more than 250,000 members.
* nopCommerce runs on .NET 9 with an MS SQL 2012 (or higher) backend database.
* nopCommerce is cross-platform, and you can run it on Windows, Linux, or Mac.
* nopCommerce supports Docker out of the box, so you can easily run nopCommerce on a Linux machine.
* nopCommerce supports PostgreSQL and MySQL databases.
* nopCommerce fully supports web farms. You can read more about it [here](https://docs.nopcommerce.com/en/developer/tutorials/web-farms.html?utm_source=github&utm_medium=referral&utm_campaign=documentation&utm_content=text).  
* All methods in nopCommerce are async.
* nopCommerce supports multi-factor authentication out of the box.
* Start our [online course for developers](https://nopcommerce.com/training?utm_source=github&utm_medium=referral&utm_campaign=course&utm_content=text) and get the practical and technical skills you need to run and customize nopCommerce websites.

![Logo](https://www.nopcommerce.com/images/github/logos.png#v2)

nopCommerce architecture follows well-known software patterns and the best security practices. The source code is fully customizable. Pluggable and clear architecture makes it easy to develop custom functionality and follow any business requirements.

Using the latest Microsoft technologies, nopCommerce provides high performance, stability, and security. nopCommerce is also fully compatible with Azure and web farms.

Our clear and detailed [documentation](https://docs.nopcommerce.com/developer/index.html?utm_source=github&utm_medium=referral&utm_campaign=documentation&utm_content=text) and [online course](https://nopcommerce.com/training?utm_source=github&utm_medium=referral&utm_campaign=course&utm_content=text) for developers will help you start with nopCommerce easily.

## Observability Architecture Notes

nopCommerce helped instrumentation work in one important way: the main storefront flows are still concentrated in a small number of orchestration points. `CatalogModelFactory` and `ProductModelFactory` sit close to user-facing behavior, so telemetry could be attached where the application still has business meaning instead of only low-level HTTP timing. That made it possible to record signals such as zero-result searches and product-details model build duration with dimensions that an operator can actually interpret. The main hindrance is that the same concentration of work also exposes how much logic is packed into broad factory methods. Search preparation, pricing, media, reviews, and plugin-driven behavior are composed inside large units with many downstream calls and only partial caching. That shape makes observability feasible, but not naturally clean: it is easy to find a place to instrument, but harder to isolate which sub-step is responsible when a metric regresses.

If I were making architectural decisions for the project going forward, I would move observability from ad hoc insertion points toward first-class boundaries. In practice, that means defining a small telemetry layer around major business operations such as search, browse, pricing, and product page composition; standardizing span names, metric names, and safe dimensions; and placing instrumentation behind decorators or pipeline-style wrappers rather than scattering it through core methods. I would also reduce high-cardinality labels by design and introduce clearer seams around expensive sub-operations so that pricing, media, and review preparation can be timed independently when needed. The cost is not trivial: more abstraction, more conventions for contributors to learn, more review overhead, and a real risk of over-instrumenting a monolith that still benefits from straightforward code paths. But the payoff is cumulative: lower friction for future telemetry, cleaner dashboards, and less guesswork during performance or reliability incidents.

The most surgical change was introducing telemetry decorators around the existing model factories instead of rewriting the factories themselves as the primary instrumentation surface. That was necessary because the two key user journeys - search and product details - are assembled inside central factory methods that already coordinate many dependencies. Instrumenting only at the controller or HTTP layer would have lost the domain meaning of the work, while instrumenting every downstream service would have created unnecessary churn. The decorator approach let the telemetry wrap existing behavior, capture duration and result shape at the right level, and leave the business code largely intact. Where a direct edit to existing code was unavoidable, it was kept narrow and mechanical: only enough registration and metric definitions were added to connect the wrappers and expose the chosen signals, without changing the functional behavior of the storefront flow.

### The advantages of working with nopCommerce ###

nopCommerce offers powerful [out-of-the-box features](https://www.nopcommerce.com/features?utm_source=github&utm_medium=referral&utm_campaign=features&utm_content=text) for creating an online store of any size and type.

nopCommerce is integrated with all the popular third-party services. You can find thousands of integrations on nopCommerce [Marketplace](https://www.nopcommerce.com/marketplace?utm_source=github&utm_medium=referral&utm_campaign=marketplace&utm_content=text).

The [Web API plugin](https://www.nopcommerce.com/web-api?utm_source=github&utm_medium=referral&utm_campaign=WebAPI&utm_content=text) by the nopCommerce team lets you build integrations with third-party services or mobile applications using REST. The Web API plugin is available with source code and covers all methods of nopCommerce: backend and frontend. You can read more about it [here](https://www.nopcommerce.com/web-api?utm_source=github&utm_medium=referral&utm_campaign=WebAPI&utm_content=text).

Friendly members of the [nopCommerce community](https://www.nopcommerce.com/boards?utm_source=github&utm_medium=referral&utm_campaign=forum&utm_content=text) will always help with advice and share their experiences. nopCommerce core development team provides [professional support](https://www.nopcommerce.com/nopcommerce-premium-support-services?utm_source=github&utm_medium=referral&utm_campaign=premium_support&utm_content=text) within 24 hours.


## Store demo ##

Evaluate the functionality and convenience of nopCommerce as a customer and store owner.

Front End | Admin area
----|------
[![ScreenShot](https://www.nopcommerce.com/images/github/public-demo.png#v1)](https://demo.nopcommerce.com?utm_source=github&utm_medium=referral&utm_campaign=demo_store&utm_content=button) | [![ScreenShot](https://www.nopcommerce.com/images/github/admin-demo.png#v1)](https://admin-demo.nopcommerce.com/admin?utm_source=github&utm_medium=referral&utm_campaign=demo_store&utm_content=button)


### nopCommerce resources ###

nopCommerce official site: [https://www.nopcommerce.com](https://www.nopcommerce.com/?utm_source=github&utm_medium=referral&utm_campaign=homepage&utm_content=links)

* [Demo store](https://www.nopcommerce.com/demo?utm_source=github&utm_medium=referral&utm_campaign=demo_store&utm_content=links)
* [Download nopCommerce](https://www.nopcommerce.com/download-nopcommerce?utm_source=github&utm_medium=referral&utm_campaign=download_nop&utm_content=links)
* [Online course for developers](https://nopcommerce.com/training?utm_source=github&utm_medium=referral&utm_campaign=course&utm_content=links)
* [Feature list](https://www.nopcommerce.com/features?utm_source=github&utm_medium=referral&utm_campaign=features&utm_content=links)
* [Web API plugin](https://www.nopcommerce.com/web-api?utm_source=github&utm_medium=referral&utm_campaign=WebAPI&utm_content=links)
* [nopCommerce documentation](https://docs.nopcommerce.com?utm_source=github&utm_medium=referral&utm_campaign=documentation&utm_content=links)
* [Community forums](https://www.nopcommerce.com/boards?utm_source=github&utm_medium=referral&utm_campaign=forum&utm_content=links)
* [Premium support services](https://www.nopcommerce.com/nopcommerce-premium-support-services?utm_source=github&utm_medium=referral&utm_campaign=premium_support&utm_content=links)
* [Certified developer program](https://www.nopcommerce.com/certified-developer-program?utm_source=github&utm_medium=referral&utm_campaign=certified_developer&utm_content=links)
* [nopCommerce partners](https://www.nopcommerce.com/partners?utm_source=github&utm_medium=referral&utm_campaign=solution_partners&utm_content=links)

nopCommerce YouTube: [The Architecture behind the nopCommerce eCommerce Platform](https://www.youtube.com/watch?v=6gLbizzSA9o&list=PLnL_aDfmRHwtJmzeA7SxrpH3-XDY2ue0a)


### Earn with nopCommerce ###

60,000 stores worldwide are powered by nopCommerce, and 10,000 new stores open every year. nopCommerce [solution partners’ directory](https://www.nopcommerce.com/partners?utm_source=github&utm_medium=referral&utm_campaign=solution_partners&utm_content=text_become_partner) gets 80,000+ page views per year from store owners who are looking for a partner to build a store from scratch, migrate from another platform, or improve and customize an existing store.

Become a solution partner of nopCommerce and get new clients – [learn more](https://www.nopcommerce.com/become-partner?utm_source=github&utm_medium=referral&utm_campaign=become-partner&utm_content=learn_more).

Create a new graphical theme or develop a new plugin or integration and sell it on the nopCommerce [Marketplace](https://www.nopcommerce.com/marketplace?utm_source=github&utm_medium=referral&utm_campaign=marketplace&utm_content=text_sell_on_marketplace).


### Contribute ###

As a free and open-source project, we are very grateful to everyone who helps us to develop nopCommerce. Please find more details about the options and bonuses for contributors at [contribute page](https://www.nopcommerce.com/contribute?utm_source=github&utm_medium=referral&utm_campaign=contribute&utm_content=text).
