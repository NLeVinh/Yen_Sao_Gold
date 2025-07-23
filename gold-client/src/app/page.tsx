'use client';

import { useState, useEffect } from 'react';
import { Button, Container, Typography, Box, Link } from '@mui/material';
import NextLink from 'next/link';

import { useAppDispatch, useAppSelector } from '@/store/hooks';
import { increment, decrement } from '@/store/slices/counterSlice';

export default function Home() {
    const value = useAppSelector((state) => state.counter.value);
    const dispatch = useAppDispatch();

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
                <Typography variant="h6" component="h6">
                    Counter: {value}
                </Typography>
                <Button onClick={() => dispatch(increment())}>+</Button>
                <Button onClick={() => dispatch(decrement())}>-</Button>
            </Box>
        </Container>
    );
}
