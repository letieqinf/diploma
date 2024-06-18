import { useState } from "react";
import { useNavigate } from "react-router-dom";

import './Register.css'
import axios from "../api/axios";

function Register() {
    const [formInput, setFormInput] = useState({
        firstName: "",
        lastName: "",
        patronymic: "",
        email: "",
        password: "",
        confirmPassword: "",
        successMessage: ""
    });

    const [formError, setFormError] = useState({
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        confirmPassword: ""
    });

    const navigate = useNavigate();

    const handleSubmit = (event: any) => {
        event.preventDefault();

        // TO-DO make validation algorithms

        let isError = false;
        let errors = {
            firstName: "",
            lastName: "",
            email: "",
            password: "",
            confirmPassword: ""
        };

        if (!formInput.firstName) {
            isError = true;
            errors.firstName = "Имя не должно быть пустым"
        };
        if (!formInput.lastName) {
            isError = true;
            errors.lastName = "Фамилия не должна быть пустой"
        };
        if (!formInput.email) {
            isError = true;
            errors.email = "Email не должен быть пустым"
        };
        if (!formInput.password) {
            isError = true;
            errors.password = "Пароль не должен быть пустым"
        };
        if (formInput.confirmPassword !== formInput.password) {
            isError = true;
            errors.confirmPassword = "Пароли не совпадают"
        };

        setFormError({...errors});

        if (!isError) {
            try {
                const response = axios.post('api/auth/register', {
                    name: formInput.firstName,
                    lastName: formInput.lastName,
                    patronymic: formInput.patronymic,
                    email: formInput.email,
                    password: formInput.password
                });

                navigate("/login");
            } catch (error) {
                setFormInput({
                    ...formInput,
                    successMessage: "Неудачная попытка регистрации"
                });
            }
        }
    };

    const handleLoginClick = () => {
        navigate("/login");
    };

    const changeHandler = (name: any, value: any) => {
        setFormInput({
            ...formInput,
            [name]: value,
        });
    };

    return (
        <div className="register-form-container">
            <div className="card">
                <div className="card-header">
                    <h3 className="title">Регистрация</h3>
                </div>

                <div className="card-body">
                    <form onSubmit={ handleSubmit }>
                        <div className="form-group">
                            <label>Фамилия</label>
                            <input 
                                value={formInput.lastName}
                                onChange={({target}) => {
                                    changeHandler(target.name, target.value)
                                }}
                                name="lastName"
                                className="input"
                            />
                        </div>
                        { formError.lastName && <p className="error-message">{ formError.lastName }</p> }
                        <div className="form-group">
                            <label>Имя</label>
                            <input 
                                value={formInput.firstName}
                                onChange={({target}) => {
                                    changeHandler(target.name, target.value)
                                }}
                                name="firstName"
                                className="input"
                            />
                        </div>
                        { formError.firstName && <p className="error-message">{ formError.firstName }</p> }
                        <div className="form-group">
                            <label>Отчество</label>
                            <input 
                                value={formInput.patronymic}
                                onChange={({target}) => {
                                    changeHandler(target.name, target.value)
                                }}
                                name="patronymic"
                                className="input"
                            />
                        </div>

                        <div className="form-group">
                            <label>Email</label>
                            <input 
                                value={formInput.email}
                                onChange={({target}) => {
                                    changeHandler(target.name, target.value)
                                }}
                                name="email"
                                type="email"
                                className="input"
                            />
                        </div>
                        { formError.email && <p className="error-message">{ formError.email }</p> }
                        <div className="form-group">
                            <label>Пароль</label>
                            <input 
                                value={formInput.password}
                                onChange={({target}) => {
                                    changeHandler(target.name, target.value)
                                }}
                                name="password"
                                type="password"
                                className="input"
                            />
                        </div>
                        { formError.password && <p className="error-message">{ formError.password }</p> }
                        <div className="form-group">
                            <label>Повторите</label>
                            <input 
                                value={formInput.confirmPassword}
                                onChange={({target}) => {
                                    changeHandler(target.name, target.value)
                                }}
                                name="confirmPassword"
                                type="password"
                                className="input"
                            />
                        </div>
                        { formError.confirmPassword && <p className="error-message">{ formError.confirmPassword }</p> }

                        { formInput.successMessage && <p className="success-message">{formInput.successMessage}</p> }

                        <button type="submit" className="submit-form">Зарегестрироваться</button>
                    </form>

                    <div className="form-login">
                        <div className="divider">
                            <span>или</span>
                        </div>
                        <button className="login-btn" onClick={handleLoginClick}>Войти</button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Register;