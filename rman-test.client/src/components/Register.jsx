import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const Register = () => {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');
    const navigate = useNavigate(); // Initialize useNavigate

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

    const handleRegister = async (e) => {
        e.preventDefault();

        console.log('Name:', name);
        console.log('Email:', email);
        console.log('Password:', password);

         if (!name || !email || !password) {
            console.error("All fields are required: Name, Email, and Password");
            return;
        }


        try {
            const response = await axios.post('https://localhost:7110/api/create', {
                name,
                email,
                password
            }, {
                headers: {
                    "Content-Type": "application/json"
                },
                withCredentials: true
            });
            setMessage(response.data.Message);

            // Log in the user after successful registration
            await loginUser(email, password);
        } catch (error) {
            setMessage('Registration failed');
        }
    };

    const loginUser = async (email, password) => {
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

            navigate('/'); // Redirect to home page after login
        } catch (error) {
            setMessage('Login failed: Invalid Email or Password');
        }
    };

    const navigateToLogin = () => {
        navigate('/login'); // Navigate to the login page
    };

    return (
        <div className="container">
            <h2>Register</h2>
            <form className="text-start" onSubmit={handleRegister}>
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Name:</label>
                    <input
                        type="text"
                        className="form-control"
                        id="name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                    />
                </div>
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
                <button type="submit" className="btn btn-primary">Register</button>
            </form>
            {message && <p>{message}</p>}
            <p className="mt-3">Already have an account? <button className="btn btn-link" onClick={navigateToLogin}>Login</button></p>
        </div>
    );
};

export default Register;
