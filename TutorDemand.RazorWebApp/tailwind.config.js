/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{cshtml,razor}",
        "./Views/**/*.{cshtml,razor}",
        "./wwwroot/js/**/*.js",
    ],
    theme: {
        extend: {
            colors: {
                primary: "#66a593",
                secondary: "#f3969a",
                success: "#56cc9d",
                info: "#6cc3d5",
                warning: "#ffce67",
                danger: "#f43f5e"
            }
        },
    },
    plugins: [],
};
