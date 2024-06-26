import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';


const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        // Check login status when the component mounts
        const checkLoginStatus = async () => {
            try {
                const response = await axios.get('https://localhost:7110/api/check', {
                    withCredentials: true
                });

                if (response.data.loggedIn) {
                    navigate('/'); // Redirect to home if already logged in
                }
            } catch (error) {
                console.error('Error checking login status:', error);
            }
        };

        checkLoginStatus();
    }, [navigate]);

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post('https://localhost:7110/api/login', {
                email: email,
                password: password
            }, {
                headers: {
                    "Content-Type": "application/json"
                },
                withCredentials: true
            });

            setMessage(response.data.Message);
            navigate('/'); // Redirect to '/'
        } catch (error) {
            setMessage('Invalid Email or Password');
        }
    };

    const handleRegisterClick = () => {
        navigate('/register'); 
    };

    return (
        <div className="container">
            <h3>Login</h3>
            <form className="text-start" onSubmit={handleLogin}>
                <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email:</label>
                    <input
                        type="email"
                        className="form-control"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="password" className="form-label">Password:</label>
                    <input
                        type="password"
                        className="form-control"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit" className="btn btn-primary">Login</button>
                <button type="button" className="btn btn-link ms-2" onClick={handleRegisterClick}>Register</button>
            </form>
            <div>{message && <p>{message}</p>}</div>
        </div>
    );
};

export default Login;
