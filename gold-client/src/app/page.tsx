'use client';

import { useState, useEffect } from 'react';
import { Button, Container, Typography, Box, Link } from '@mui/material';
import NextLink from 'next/link';

import { useAppDispatch, useAppSelector } from '@/store/hooks';
import { increment, decrement } from '@/store/slices/counterSlice';
import { getUsers } from '@/services/userService';
import { User } from '@/types/User';

export default function Home() {
    const [users, setUsers] = useState<User[]>([]);
    const value = useAppSelector((state) => state.counter.value);
    const dispatch = useAppDispatch();

    useEffect(() => {
        const fetUsers = async () => {
            try {
                const res = await getUsers();
                setUsers(res);
            } catch (error) {
                console.log(error);
            }
        };
        fetUsers();
    });

    return (
        <Container maxWidth="lg" style={{ marginTop: '100px' }}>
            <Box
                sx={{
                    my: 4,
                    display: 'flex',
                    flexDirection: 'column',
                    justifyContent: 'center',
                    alignItems: 'center',
                }}
            >
                <Typography variant="h4" component="h1" sx={{ mb: 2 }}>
                    Material UI - Next.js App Router example in TypeScript
                </Typography>
                <Link href="/about" color="secondary" component={NextLink}>
                    Go to the about page
                </Link>
            </Box>

            <Box>
                <Typography variant="h1" component="h1">
                    Counter: {value}
                </Typography>
                <Button onClick={() => dispatch(increment())}>+</Button>
                <Button onClick={() => dispatch(decrement())}>-</Button>
            </Box>
            <Box>
                <ul>
                    {users.map((user) => (
                        <li key={user.id}>
                            {user.name} ({user.email})
                        </li>
                    ))}
                </ul>
            </Box>
        </Container>
    );
}
