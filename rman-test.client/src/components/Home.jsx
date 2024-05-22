import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom'; // Import useNavigate hook

const Home = () => {
    const [loggedIn, setLoggedIn] = useState(false);
    const [userName, setUserName] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        const checkLoginStatus = async () => {
            try {
                const response = await axios.get('https://localhost:7110/api/check', {
                    withCredentials: true
                });
                setLoggedIn(response.data.loggedIn);
                if (response.data.loggedIn) {
                    setUserName(response.data.userName); // Set the user's name
                }
            } catch (error) {
                console.error('Error checking login status:', error);
                setLoggedIn(false);
            }
        };
        checkLoginStatus();
    }, []);

    const handleLogout = async () => {
        try {
            const response = await axios.post('https://localhost:7110/api/logout', {}, {
                withCredentials: true
            });
            console.log(response.data);
            setLoggedIn(false);
        } catch (error) {
            console.error('Error logging out:', error);
        }
    };

    const handleLogin = () => {
        navigate('/login');
    };

    return (
        <div className="container mt-5">
            <h2 className="mb-4">Home</h2>
            {loggedIn ? (
                <div>
                    <p className="mb-3">Welcome back, {userName}!</p> {/* Display the user's name */}
                    <button className="btn btn-primary btn-lg mr-2" onClick={handleLogout}>Logout</button>
                </div>
            ) : (
                <div>
                    <p>Please log in.</p>
                    <button className="btn btn-success btn-lg mr-2" onClick={handleLogin}>Login</button>
                </div>
            )}
        </div>
    );
};

export default Home;

