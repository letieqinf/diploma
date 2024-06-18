import { Link, NavLink, useNavigate } from "react-router-dom";

import "./AuthorizedHeader.css";

import useLogout from "../../../../hooks/useLogout";

function AuthorizedHeader() {
    const navigate = useNavigate();
    const logout = useLogout();

    const signOut = async () => {
        await logout();
        navigate('/login');
    }

    return (
        <header className="authorized-header">
            <div className="authorized-header-container authorized-header-menu-container">
                <ul className="authorized-header-menu">
                    <li className="account-management">
                        <Link to="/profile" className="profile-trigger">Профиль</Link>
                        <Link to="/login" className="signout-trigger">Выйти</Link>
                    </li>
                </ul>
            </div>
            <div className="authorized-header-container authorized-header-sub-container">
                <ul className="authorized-header-sub">
                    <li className="tusur-logo-container">
                        <img src="/assets/tusur-logo.svg" />
                    </li>
                    <li className="page-title-container">
                        <p>Кабинет практик</p>
                    </li>
                </ul>
            </div>
            <div className="authorized-header-container authorized-header-bottom-container">
                <ul className="authorized-header-bottom">
                    <li className="bottom-item">
                        <NavLink to="/" className={({ isActive }) => {
                            return (isActive ? 'active' : '');
                        }}>Главная</NavLink>
                    </li>
                    <li className="bottom-item">
                        <NavLink to="/documents" className={({ isActive }) => {
                            return (isActive ? 'active' : '');
                        }}>Документы</NavLink>
                    </li>
                </ul>
            </div>
        </header>
    );
}

export default AuthorizedHeader;