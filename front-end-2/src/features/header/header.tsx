import "./header.scss";
import { Link } from "react-router-dom";

function Header() {
  return (
    <header>
      <Link 
            data-content={process.env.REACT_APP_API_AUTH_ENDPOINT}
            to="/">
            {process.env.REACT_APP_API_AUTH_ENDPOINT}
      </Link>
    </header>
  );
}

export { Header };
