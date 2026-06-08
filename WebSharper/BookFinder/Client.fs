namespace BookFinder

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Client =

    type SearchSource = {
        Name        : string
        Icon        : string
        Description : string
        Url         : string
        IsBest      : bool
    }

    type SearchResult = {
        Title  : string
        Author : string
    }

    let popularBooks = [
        ("Clean Code",               "Robert Martin")
        ("The Great Gatsby",         "F. Scott Fitzgerald")
        ("Atomic Habits",            "James Clear")
        ("1984",                     "George Orwell")
        ("Harry Potter",             "J.K. Rowling")
        ("The Pragmatic Programmer", "David Thomas")
        ("Deep Work",                "Cal Newport")
        ("Thinking Fast and Slow",   "Daniel Kahneman")
    ]

    let titleVar    = Var.Create ""
    let authorVar   = Var.Create ""
    let resultsVar  = Var.Create ([] : SearchSource list)
    let historyVar  = Var.Create ([] : SearchResult list)
    let searchedVar = Var.Create false
    let loadingVar  = Var.Create false
    let msgVar      = Var.Create ""

    let buildSources (title: string) (author: string) =
        let enc (s: string) = JS.EncodeURIComponent(s)
        let q  = enc (title + " " + author)
        let qt = enc title
        let qa = enc author
        [
            { Name = "Internet Archive";  Icon = "📚"; Description = "✅ Best source — Free digital library, millions of books"; Url = "https://archive.org/search?query=" + q + "&and[]=mediatype%3A%22texts%22"; IsBest = true  }
            { Name = "Library Genesis";   Icon = "📖"; Description = "Largest free book database";                               Url = "https://libgen.is/search.php?req=" + q + "&res=25&view=simple";            IsBest = false }
            { Name = "PDF Drive";         Icon = "📄"; Description = "80 million+ free PDF books";                              Url = "https://www.pdfdrive.com/search?q=" + q;                                   IsBest = false }
            { Name = "Z-Library";         Icon = "📗"; Description = "World's largest e-book library";                          Url = "https://z-lib.id/s/" + q;                                                  IsBest = false }
            { Name = "Open Library";      Icon = "🌐"; Description = "Open access to millions of books";                        Url = "https://openlibrary.org/search?q=" + q;                                    IsBest = false }
            { Name = "Google PDF Search"; Icon = "🔍"; Description = "Search Google for PDF files directly";                    Url = "https://www.google.com/search?q=filetype%3Apdf+" + qt + "+" + qa;         IsBest = false }
            { Name = "Bing PDF Search";   Icon = "🔎"; Description = "Bing search filtered for PDF documents";                  Url = "https://www.bing.com/search?q=filetype%3Apdf+" + q;                        IsBest = false }
            { Name = "DuckDuckGo PDF";    Icon = "🦆"; Description = "Privacy-friendly PDF search";                            Url = "https://duckduckgo.com/?q=filetype%3Apdf+" + q;                            IsBest = false }
        ]

    let doSearch () =
        let title  = titleVar.Value.Trim()
        let author = authorVar.Value.Trim()
        if title = "" then
            msgVar.Value <- "⚠️ Please enter a book title!"
        else
            msgVar.Value      <- ""
            loadingVar.Value  <- true
            searchedVar.Value <- false

            JS.SetTimeout (fun () ->
                let sources = buildSources title author
                resultsVar.Value  <- sources
                loadingVar.Value  <- false
                searchedVar.Value <- true

                let best = sources |> List.tryFind (fun s -> s.IsBest)
                match best with
                | Some s -> JS.Window.Open(s.Url, "_blank") |> ignore
                | None   -> ()

                let entry = { Title = title; Author = if author = "" then "Unknown" else author }
                historyVar.Value <-
                    entry :: historyVar.Value
                    |> List.distinctBy (fun h -> h.Title)
                    |> List.truncate 8
            ) 1800 |> ignore

    let setPopular (title: string) (author: string) =
        titleVar.Value  <- title
        authorVar.Value <- author
        doSearch ()

    [<SPAEntryPoint>]
    let Main () =

        div [ attr.``class`` "app" ] [

            // ── HEADER ──
            div [ attr.``class`` "header" ] [
                div [ attr.``class`` "header-top" ] [
                    span [ attr.``class`` "label-left"  ] [ text "VEL6D3" ]
                    span [ attr.``class`` "label-right" ] [ text "Fahad Muhammad" ]
                ]
                div [ attr.``class`` "header-icon" ] [ text "📚" ]
                h1 [] [ text "BookFinder" ]
                p [ attr.``class`` "subtitle" ] [ text "Find any book PDF instantly from 8 sources" ]
                p [ attr.``class`` "author-credit" ] [ text "by Fahad Muhammad" ]
            ]

            // ── SEARCH BOX ──
            div [ attr.``class`` "search-card" ] [
                div [ attr.``class`` "search-row" ] [
                    div [ attr.``class`` "input-wrap" ] [
                        span [ attr.``class`` "input-icon" ] [ text "📖" ]
                        Doc.InputType.Text [
                            attr.placeholder "Enter book title..."
                            attr.``class``   "search-input"
                        ] titleVar
                    ]
                    div [ attr.``class`` "input-wrap" ] [
                        span [ attr.``class`` "input-icon" ] [ text "✍️" ]
                        Doc.InputType.Text [
                            attr.placeholder "Author name (optional)..."
                            attr.``class``   "search-input"
                        ] authorVar
                    ]
                ]
                button [
                    attr.``class`` "search-btn"
                    on.click (fun _ _ -> doSearch ())
                ] [ text "🔍  Find PDF" ]
                p [ attr.``class`` "msg" ] [ textView msgVar.View ]
            ]

            // ── LOADING ANIMATION ──
            loadingVar.View |> Doc.BindView (fun loading ->
                if not loading then div [] [] :> Doc
                else
                    div [ attr.``class`` "loading-section" ] [
                        div [ attr.``class`` "loading-spinner" ] []
                        p [ attr.``class`` "loading-text" ] [ text "🔍 Searching across all sources..." ]
                        p [ attr.``class`` "loading-sub"  ] [ text "Opening best result automatically..." ]
                    ] :> Doc
            )

            // ── QUICK SEARCH ──
            div [ attr.``class`` "popular-section" ] [
                p [ attr.``class`` "section-title" ] [ text "⚡ Quick Search" ]
                div [ attr.``class`` "popular-grid" ] [
                    popularBooks |> List.map (fun (title, author) ->
                        button [
                            attr.``class`` "popular-btn"
                            on.click (fun _ _ -> setPopular title author)
                        ] [
                            span [ attr.``class`` "pop-title"  ] [ text title  ]
                            span [ attr.``class`` "pop-author" ] [ text author ]
                        ] :> Doc
                    ) |> Doc.Concat
                ]
            ]

            // ── RESULTS ──
            searchedVar.View |> Doc.BindView (fun searched ->
                if not searched then div [] [] :> Doc
                else
                    div [ attr.``class`` "results-section" ] [
                        titleVar.View |> Doc.BindView (fun t ->
                            div [ attr.``class`` "results-header" ] [
                                p [ attr.``class`` "section-title" ] [
                                    text ("📄 Results for: \"" + t + "\"")
                                ]
                                p [ attr.``class`` "results-hint" ] [
                                    text "✅ Best source opened automatically — use cards below as backup"
                                ]
                            ] :> Doc
                        )
                        div [ attr.``class`` "results-grid" ] [
                            resultsVar.View |> Doc.BindView (fun sources ->
                                sources |> List.map (fun s ->
                                    div [ attr.``class`` (if s.IsBest then "result-card best-card" else "result-card") ] [
                                        div [ attr.``class`` "result-top" ] [
                                            span [ attr.``class`` "result-icon" ] [ text s.Icon ]
                                            div [ attr.``class`` "result-info" ] [
                                                strong [] [ text s.Name ]
                                                p [ attr.``class`` "result-desc" ] [ text s.Description ]
                                            ]
                                        ]
                                        a [
                                            attr.href      s.Url
                                            attr.target    "_blank"
                                            attr.rel       "noopener noreferrer"
                                            attr.``class`` (if s.IsBest then "open-btn best-btn" else "open-btn")
                                        ] [ text (if s.IsBest then "⭐ Open Best Source →" else "🔗 Open →") ]
                                    ] :> Doc
                                ) |> Doc.Concat
                            )
                        ]
                    ] :> Doc
            )

            // ── HISTORY ──
            historyVar.View |> Doc.BindView (fun history ->
                if history.IsEmpty then div [] [] :> Doc
                else
                    div [ attr.``class`` "history-section" ] [
                        p [ attr.``class`` "section-title" ] [ text "🕐 Recent Searches" ]
                        div [ attr.``class`` "history-list" ] [
                            history |> List.map (fun h ->
                                button [
                                    attr.``class`` "history-btn"
                                    on.click (fun _ _ -> setPopular h.Title h.Author)
                                ] [
                                    span [] [ text h.Title ]
                                    span [ attr.``class`` "history-author" ] [ text h.Author ]
                                ] :> Doc
                            ) |> Doc.Concat
                        ]
                    ] :> Doc
            )

            // ── FOOTER ──
            div [ attr.``class`` "footer" ] [
                p [ attr.``class`` "footer-dim" ] [ text "📚 BookFinder — Built with F# & WebSharper" ]
                p [] [
                    span [ attr.``class`` "footer-dim" ] [ text "University of Dunaújváros  •  " ]
                    span [ attr.``class`` "footer-name" ] [ text "Fahad Muhammad" ]
                    span [ attr.``class`` "footer-dim" ] [ text "  •  VEL6D3" ]
                ]
            ]
        ]
        |> Doc.RunById "main"