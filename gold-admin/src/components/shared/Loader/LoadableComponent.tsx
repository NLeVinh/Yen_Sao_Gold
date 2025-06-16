import { ComponentType, Suspense } from 'react'

// project import
import Loader from './LoaderComponent'

const Loadable =
    <P extends object>(Component: ComponentType<P>): React.FC<P> =>
    (props: P) =>
        (
            <Suspense fallback={<Loader />}>
                <Component {...props} />
            </Suspense>
        )

export default Loadable
