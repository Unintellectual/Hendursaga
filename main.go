package main

import (
	"embed"
	"hendursaga/components"
	"log"
	"net/http"

	"github.com/a-h/templ"
)

var static embed.FS

//go:generate npx tailwindcss build -i static/css/style.css -o static/css/tailwind.css -m

func main() {
	homePage := components.Index()
	pagesHandler := http.NewServeMux()
	pagesHandler.Handle("/", templ.Handler(homePage))
	pagesHandler.Handle("/static/", http.FileServer(http.FS(static)))

	log.Fatalln(http.ListenAndServe(":8080", pagesHandler))
}
