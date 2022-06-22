import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import "../../locales/i18n";
import { useAppSelector } from "../../storage";
import "./footer.scss";

function Footer() {
  const { t } = useTranslation();
  const identity = useAppSelector(x => x.identity.value);

  return (
    <footer>
      <div className="footer-content">
        <Link to="/login" title={t("Account.Login")} aria-label={t("Account.Login")}>
          <svg className={`footer-content-tool ${identity === null ? "" : "active"}`}
               version="1.1" viewBox="0 0 302 302" xmlns="http://www.w3.org/2000/svg">
                <path d="m266.12 0c-9.6062-0.028617-45.569 0.126-114.63 0-61.193 0.126-105.51 0.019413-111.91 0-5.522 30.225-16.649 149.32-16.649 149.32 0 51.596 83.36 126.99 115.95 153.04 12.862 5e-3 14.224-0.0187 25.427 0 32.603-26.067 115.73-101.46 115.73-153.04 0 1e-3 -8.3997-119.1-13.923-149.32zm-19.63 159.71v6h-190s3e-6 -2.687 0-6l-3.9e-5 -57.107c-2e-6 -3.313 3.9e-5 -6.0006 3.9e-5 -6.0006h190v6z"/>
                <g>
                  <path d="m86.646 111.25v13.102l12.044-4.1504 3.6621 9.6029-12.044 5.0456 7.5684 10.742-8.138 6.0222-8.2194-10.824-8.0567 10.824-8.5449-6.0222 7.6498-11.149-12.126-4.6387 3.4994-9.6029 12.451 4.1504v-13.102z"/>
                  <path d="m133.44 111.25v13.102l12.044-4.1504 3.6621 9.6029-12.044 5.0456 7.5684 10.742-8.138 6.0222-8.2194-10.824-8.0566 10.824-8.5449-6.0222 7.6498-11.149-12.126-4.6387 3.4994-9.6029 12.451 4.1504v-13.102z"/>
                  <path d="m180.23 111.25v13.102l12.044-4.1504 3.6621 9.6029-12.044 5.0456 7.5684 10.742-8.138 6.0222-8.2194-10.824-8.0567 10.824-8.5449-6.0222 7.6498-11.149-12.126-4.6387 3.4994-9.6029 12.451 4.1504v-13.102z"/>
                  <path d="m227.03 111.25v13.102l12.044-4.1504 3.6621 9.6029-12.044 5.0456 7.5684 10.742-8.138 6.0222-8.2194-10.824-8.0567 10.824-8.5449-6.0222 7.6498-11.149-12.126-4.6387 3.4994-9.6029 12.451 4.1504v-13.102z"/>
                </g>
          </svg>
        </Link>

        <div className="footer-content-copyright">
          {t("Footer.Copyright")} © {new Date().getFullYear()}
        </div>
      </div>
    </footer>
  );
}

export { Footer };