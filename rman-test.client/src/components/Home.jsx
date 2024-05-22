import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { BoxArrowRight } from 'react-bootstrap-icons'
const Home = () => {
    const [loggedIn, setLoggedIn] = useState(false);
    const [userName, setUserName] = useState('');
    const [users, setUsers] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get('https://localhost:7110/api/check', {
                    withCredentials: true
                });
                setLoggedIn(response.data.loggedIn);
                if (response.data.loggedIn) {
                    setUserName(response.data.userName);
                }
            } catch (error) {
                console.error('Error checking login status:', error);
                setLoggedIn(false);
            }
        };
        fetchData();
    }, []);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get('https://localhost:7110/api/users', {
                    withCredentials: true
                });
                setUsers(response.data);
            } catch (error) {
                console.error('Error fetching users:', error);
            }
        };
        if (loggedIn) {
            fetchUsers();
        }
    }, [loggedIn]);

    const handleLogout = async () => {
        try {
            const response = await axios.post('https://localhost:7110/api/logout', {}, {
                withCredentials: true
            });
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
                    <p className="mb-3">Welcome back, {userName}!</p>
                    <div className="d-flex justify-content-end">
                        <button className="" onClick={handleLogout}><BoxArrowRight /></button>
                    </div>
                    <div>
                        <h4>Users List</h4>
                        <div className="table-responsive">
                            <table className="table table-striped">
                                <thead className="thead-dark">
                                    <tr>
                                        <th>ID</th>
                                        <th>Username</th>
                                        <th>Email</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {users.map(user => (
                                        <tr key={user.id}>
                                            <td>{user.id}</td>
                                            <td>{user.userName}</td>
                                            <td>{user.email}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
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
