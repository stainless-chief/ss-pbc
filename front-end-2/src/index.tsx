import ReactDOM from "react-dom/client";
import "./index.scss";
import { reportWebVitals } from "./reportWebVitals";
import { Footer, Header, Navigation } from "./features"
import { BrowserRouter } from "react-router-dom";
import { AppRouter } from "./AppRouter";
import { ThemeProvider, createTheme } from "@mui/material/styles";

const darkTheme = createTheme({
  palette: {
    mode: "dark",
  },
});

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

root.render(
  <ThemeProvider theme={darkTheme}>
    <BrowserRouter>
      <Header/>
      <Navigation />
      <AppRouter />
    </BrowserRouter>
    <Footer/> 
  </ThemeProvider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
