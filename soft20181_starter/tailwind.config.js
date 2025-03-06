/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.{html,cshtml}",
    "./wwwroot/js/site.js",
    "./Views/**/*.{html,cshtml}",
    "./_ViewImports.cshtml",
    "./_ViewStart.cshtml",
    "./wwwroot/css/site.css",
    "./wwwroot/css/output.css",
    "./wwwroot/lib/bootstrap/dist/css/bootstrap-grid.css",
    "./wwwroot/lib/bootstrap/dist/css/bootstrap-grid.css.map",
  ],
  theme: {
    extend: {
      colors: {
        red: "#FC4747",
        darkBlue: "#10141E",
        lightGrey: "#5A698F",
        lightDark: "#161D2F",
        white : "#FFFFFF"
      }
    },
  },
  plugins: [],
}

