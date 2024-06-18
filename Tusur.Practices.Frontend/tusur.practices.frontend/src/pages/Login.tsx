import { useRef, useState, useEffect, useContext } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';

import AuthContext from '../context/AuthProvider';
import axios from '../api/axios';

import './Login.css'

const LOGIN_URL = '/api/auth/login';

function Login() {
    const { setAuth } = useContext(AuthContext);

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";

    // Variables for login window items
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    // Variable for error messages
    const [message, setMessage] = useState<string>("");

    // Handle changes
    const handleChanges = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        if (name === "email") setEmail(value);
        if (name === "password") setPassword(value);
    };

    // Handle register button click
    const handleRegisterClick = () => {
        navigate('/register')
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!email || !password) {
            setMessage("Не все поля заполнены.");
        } else {
            try {
                const response = await axios.post(LOGIN_URL,
                    JSON.stringify({ email, password }),
                    {
                        headers: { 'Content-Type': 'application/json' },
                        withCredentials: true
                    }
                );
                
                const accessToken = response?.data?.accessToken;
                const roles = response?.data?.roles;
                setAuth({ email, password, roles, accessToken });

                setMessage("");

                navigate(from, { replace: true });
            } catch (error) {
                if (!error?.response) {
                    setMessage('Нет ответа от сервера');
                } else {
                    setMessage('Неудачная попытка входа')
                }
            };
        }
    };

    return (
        <div className="login-form-container">
            <div className="card">
                <div className="title">
                    <h3>Войти</h3>
                </div>
                <div className="card-body">
                    <form onSubmit={ handleSubmit }>
                        <div className="form-group">
                            <label htmlFor="email">Email</label>
                            <input
                                className="input"
                                type="email"
                                id="email"
                                name="email"
                                value={ email }
                                onChange={ handleChanges }
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="password">Пароль</label>
                            <input
                                className="input"
                                type="password"
                                id="password"
                                name="password"
                                value={ password }
                                onChange={ handleChanges }
                            />
                        </div>
                        <button type="submit" className="submit-form">Войти</button>
                    </form>
                    <div className="form-register">
                        <div className="divider">
                            <span>или</span>
                        </div>
                        <button onClick={handleRegisterClick} className="register-btn">Зарегестрироваться</button>
                    </div>
                </div>
                { message && <p className="message">{ message }</p> }
            </div>
        </div>
    );
}

export default Login;