import { Route, Routes } from "react-router-dom";
import * as Pages from "./pages";

const AppRouter = () => (
  <Routes>
    <Route path="/" element={<Pages.CoreLayout />} >
      <Route path="*" element={<Pages.ErrorNotFoundPage />} />

      <Route index element={<Pages.IntroductionPage />} />

      <Route path="projects/:projectCode" element={<Pages.ProjectPage />} />

      <Route path="projects" element={<Pages.ShowcasePage />}>
        <Route path="?category=:categoryCode" element={<Pages.ShowcasePage />} />
        <Route path="?category=:categoryCode&page=:page" element={<Pages.ShowcasePage />} />
      </Route>

      <Route path="login" element={<Pages.LoginPage />} />

      <Route path="admin/intro" element={<Pages.AdminIntroductionPage />} />
      <Route path="admin/accounts" element={<Pages.AdminAccountsPage />} />
      <Route path="admin/projects" element={<Pages.AdminProjectsPage />} />
      <Route path="admin/categories" element={<Pages.AdminCategoriesPage />} />
    </Route>
  </Routes>
);

export { AppRouter };
