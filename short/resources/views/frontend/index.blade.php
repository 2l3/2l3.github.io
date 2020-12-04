@extends('frontend.layout')

@section('content')
    <header class="masthead text-white text-center" style="background-image: url('{{ Custom::getBackground() }}');">
        <div class="container">
            <div class="row full-height align-items-center">
                <div class="col-12">
                    <div class="row">
                        <div class="col-xl-9 mx-auto">
                            <h1 class="mb-5">{{ setting('homepage_description') }}</h1>
                        </div>
                        <div class="col-md-10 col-lg-8 col-xl-7 mx-auto">
                            <short-link></short-link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </header>
@endsection
