/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      fontFamily: {
        mono: ['Courier New', 'Courier', 'monospace'],
      },
      colors: {
        military: {
          black: '#050505',
          dark: '#111111',
          green: '#10b981',
          red: '#ef4444', 
        }
      }
    },
  },
  plugins: [],
}